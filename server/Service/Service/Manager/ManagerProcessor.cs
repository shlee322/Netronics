using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using Netronics;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Event;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Bson;
using Netronics.Protocol.PacketEncryptor;
using Newtonsoft.Json.Linq;
using Service.Service.Loader;

namespace Service.Service.Manager
{
    internal class ManagerProcessor
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

        public ManagerProcessor(ServiceLoader loader, string[] host)
        {
            _loader = loader;
            _host = host;
            _processor = new Thread(Processing);
            _processor.Start();
            _netronics = new Netronics.Netronics(new Properties.Properties(new IPEndPoint(IPAddress.Any, 0), () => new Handler.Service(this)));
            _netronics.Start();

            Connect();
            _manager = new Client(new Properties.Properties(_managerIPEndPoint, () => new Handler.Manager(this)));
            _manager.Start();
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
                        _services[item.Service].NotifyJoinService(item.Id, item.Address, item.Port);
                    }
                }
                Thread.Sleep(0);
            }
        }

        public void NotifyJoinService(string service, int id, byte[] address, int port)
        {
            _addremoveQueue.Enqueue(new AddRemoveItem(){IsAdd = true, Service = service, Id = id, Address = address, Port = port});
        }
    }
}