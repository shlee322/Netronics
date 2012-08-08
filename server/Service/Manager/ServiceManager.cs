﻿using System;
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
        private NetworkManager _networkManager;

        public ServiceManager(string path)
        {
            _networkManager = new NetworkManager(path);
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

        public void JoinService(IChannel channel, string name, int id, JArray address)
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

            var service = services.JoinService(channel, id, address);

            if (id == -1)
            {
                var networks = _networkManager.GetNetworks(service);
                foreach (var network in networks)
                {
                    var remoteServiceName = network.Service1 == name ? network.Service2 : network.Service1;
                    var remoteService = GetServices(remoteServiceName);
                    if(remoteService == null)
                        continue;
                    channel.SendMessage(remoteService.GetServicesNetworkInfo(network));
                }
            }
        }
    }
}