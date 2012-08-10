using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using Netronics;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;
using Service.Service.Loader;
using Service.Service.Task;

namespace Service.Service.Manager
{
    public class ManagerProcessor
    {

        private readonly string[] _host;

        private readonly Netronics.Netronics _manager;
        private ServiceLoader _loader;

        private IPEndPoint _managerIPEndPoint;

        private int _serviceId = 0;

        private readonly ReaderWriterLockSlim _servicesLock = new ReaderWriterLockSlim();
        private Dictionary<string, Services> _services = new Dictionary<string, Services>();
        private ConcurrentQueue<AddRemoveItem> _addremoveQueue = new ConcurrentQueue<AddRemoveItem>();
        private Thread _processor;

        private Netronics.Netronics _netronics;

        private Task.Scheduler[] _scheduler = new Scheduler[4];

        private long _maxEntityId;

        public ManagerProcessor(ServiceLoader loader, string[] host)
        {
            _loader = loader;
            _host = host;
            _processor = new Thread(Processing);
            _processor.Start();
            for (int i = 0; i < 4; i++)
                _scheduler[i] = new Scheduler();
            _loader.GetService().SetManagerProcessor(this);
            _loader.GetService().Load(this);

            _netronics = new Netronics.Netronics(new Properties.Properties(new IPEndPoint(IPAddress.Any, 0), () => new Handler.Service(this)));
            _netronics.Start();

            Connect();
            _manager = new Client(new Properties.Properties(_managerIPEndPoint, () => new Handler.Manager(this)));
            _manager.Start();

            _loader.GetService().Start();
        }

        public int GetPort()
        {
            return _netronics.GetEndIPPoint().Port;
        }

        private List<byte[]> GetIPAddress()
        {
            var addresses = new List<byte[]>();

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                foreach (UnicastIPAddressInformation uniCast in properties.UnicastAddresses)
                {
                    if (!IPAddress.IsLoopback(uniCast.Address))
                        addresses.Add(uniCast.Address.GetAddressBytes());
                }
            }
            return addresses;
        }

        private bool Connect()
        {
            string host = GetHost();
            if (host == null)
                return false;
            _managerIPEndPoint = new IPEndPoint(IPAddress.Parse(host), 1005);
            return true;
        }

        private string GetHost()
        {
            foreach (string host in _host)
            {
                try
                {
                    var clnt = new TcpClient(host, 1005);
                    clnt.Close();
                    return host;
                }
                catch
                {
                }
            }
            return null;
        }

        public object GetJoinServicePacket()
        {
            dynamic packet = new JObject();
            packet.type = "join_service";
            if (_serviceId != 0)
                packet.id = _serviceId;
            packet.name = _loader.GetServiceName();
            packet.address = new JArray(GetIPAddress());
            packet.port = GetPort();
            return packet;
        }

        private void Processing()
        {
            while (true)
            {
                AddRemoveItem item;
                _addremoveQueue.TryDequeue(out item);
                if (item != null)
                {
                    if (item.IsAdd)
                    {
                        if(!_services.ContainsKey(item.Service))
                        {
                            var services = new Dictionary<string, Services>(_services);
                            services.Add(item.Service, new Services(this, item.Service));
                            _services = services;
                        }
                        if (item.Channel == null)
                            _services[item.Service].NotifyJoinService(item.Id, item.Address, item.Port);
                        else
                            _services[item.Service].ConnectServiceInfo(item.Id, item.Channel);
                    }
                }
                Thread.Sleep(0);
            }
        }

        public void NotifyJoinService(string service, int id, byte[] address, int port)
        {
            _addremoveQueue.Enqueue(new AddRemoveItem(){IsAdd = true, Service = service, Id = id, Address = address, Port = port});
        }

        public void ChangeServiceId(int id)
        {
            _serviceId = id;
        }

        public ServiceLoader GetServiceLoader()
        {
            return _loader;
        }

        public int GetServiceId()
        {
            return _serviceId;
        }

        public void ConnectServiceInfo(IChannel channel,string service, int id)
        {
            _addremoveQueue.Enqueue(new AddRemoveItem() { IsAdd = true, Service = service, Id = id, Channel = channel });
        }

        public void ChangeMaxEntityId(long value)
        {
            _maxEntityId = value;
        }

        public void LoadTask(int type, Func<IEnumerator<Task.Task>> func)
        {
        }

        public void Call(long uid, Func<IEnumerator<Task.Task>> func)
        {
            _scheduler[uid % _scheduler.Length].Add(new Task.Task(func));
        }

        public Services GetServices(string name)
        {
            var services = _services;
            return services.ContainsKey(name) ? services[name] : null;
        }
    }
}