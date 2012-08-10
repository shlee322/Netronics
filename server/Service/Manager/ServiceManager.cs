using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
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

namespace Service.Manager
{
    internal class ServiceManager : IProperties, IChannelPipe, IProtocol
    {
        private static ServiceManager _manager;

        private static readonly BsonEncoder Encoder = new BsonEncoder();
        private static readonly BsonDecoder Decoder = new BsonDecoder();

        private readonly Netronics.Netronics _netronics;
        private readonly ReaderWriterLockSlim _servicesLock = new ReaderWriterLockSlim();
        private readonly Dictionary<string, Services> _services = new Dictionary<string, Services>();
        private ConcurrentQueue<AddRemoveItem> _addremoveQueue = new ConcurrentQueue<AddRemoveItem>();
        private NetworkManager _networkManager;
        private Thread _processor;

        private AutoResetEvent _nextAddRemoveProcessing = new AutoResetEvent(false);
        private Service _addremoveService = null;
        private int _notifyServiceCount = -1;


        public ServiceManager(string path)
        {
            _networkManager = new NetworkManager(path);
            _processor = new Thread(Processing);
            _processor.Start();
            _netronics = new Netronics.Netronics(this);
            _netronics.Start();
        }

        #region IChannelPipe Members

        public IChannel CreateChannel(Netronics.Netronics netronics, Socket socket)
        {
            SocketChannel channel = SocketChannel.CreateChannel(socket);
            channel.SetProtocol(this);
            channel.SetHandler(new Handler(this));
            return channel;
        }

        #endregion

        #region IProperties Members

        public void OnStartEvent(Netronics.Netronics netronics, StartEventArgs eventArgs)
        {
        }

        public void OnStopEvent(Netronics.Netronics netronics, EventArgs eventArgs)
        {
        }

        public IPEndPoint GetIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Any, 1005);
        }

        public IChannelPipe GetChannelPipe()
        {
            return this;
        }

        #endregion

        #region IProtocol Members

        public IPacketEncryptor GetEncryptor()
        {
            return null;
        }

        public IPacketDecryptor GetDecryptor()
        {
            return null;
        }

        public IPacketEncoder GetEncoder()
        {
            return Encoder;
        }

        public IPacketDecoder GetDecoder()
        {
            return Decoder;
        }

        #endregion

        public static void Load(string path)
        {
            _manager = new ServiceManager(path);
        }

        private Services GetServices(string name)
        {
            Services services = null;
            _servicesLock.EnterReadLock();
            if (_services.ContainsKey(name))
                services = _services[name];
            _servicesLock.ExitReadLock();
            return services;
        }

        private void Processing()
        {
            while (true)
            {
                AddRemoveItem item;
                _addremoveQueue.TryDequeue(out item);
                if(item != null)
                {
                    if (item.IsAdd)
                        AddService(item.Channel, item.Services, item.Id, item.Address, item.Port);
                }
                Thread.Sleep(0);
            }
        }

        private void AddService(IChannel channel, Services services, int id, JArray address, int port)
        {
            var service = services.JoinService(channel, id, address, port);

            if (id == -1)
            {
                dynamic packet = new JObject();
                packet.type = "max_entity_id";
                packet.value = 10000;
                channel.SendMessage(packet);

                var networks = _networkManager.GetNetworks(service);

                int notifyServiceCount = 0;
                foreach (var network in networks)
                {
                    var remoteServiceName = network.Service1 == services.GetServicesName() ? network.Service2 : network.Service1;
                    var remoteService = GetServices(remoteServiceName);
                    if (remoteService == null)
                        continue;
                    notifyServiceCount += remoteService.NotifyJoinService(service, network);
                }
                //Interlocked.Increment
                //여기서 광역 락 시전!
                //추가 됬다고 정보를 알린 모든 서비스에서 승인이 떨어지면 다음꺼 처리.
                if (notifyServiceCount > 0)
                {
                    _addremoveService = service;
                    _notifyServiceCount = notifyServiceCount;
                    _nextAddRemoveProcessing = new AutoResetEvent(false);
                    _nextAddRemoveProcessing.WaitOne();
                }
            }
        }

        public void JoinService(IChannel channel, string name, int id, JArray address, int port)
        {
            var services = GetServices(name);
            if(services == null)
            {
                _servicesLock.EnterWriteLock();
                if (!_services.ContainsKey(name))
                {
                    services = new Services(name);
                    _services.Add(name, services);
                }
                else
                {
                    services = _services[name];
                }
                _servicesLock.ExitWriteLock();
            }

            _addremoveQueue.Enqueue(new AddRemoveItem() { IsAdd = true, Channel = channel, Address = address, Port=port,  Id = id, Services = services });
        }
    }
}