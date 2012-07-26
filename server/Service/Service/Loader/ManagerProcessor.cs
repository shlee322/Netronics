using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Netronics;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncryptor;

namespace Service.Service.Loader
{
    class ManagerProcessor : IProperties, IChannelPipe, IProtocol
    {
        private readonly Netronics.Netronics _manager;
        private ServiceLoader _loader;

        private readonly string[] _host;
        private IPEndPoint _managerIPEndPoint;

        public ManagerProcessor(ServiceLoader loader, string[] host)
        {
            _loader = loader;
            _host = host;
            _manager = new Client(this);
            var address = GetIPAddress();

            Connect();
            _manager.Start();
        }

        private List<IPAddress> GetIPAddress()
        {
            var addresses = new List<IPAddress>();

            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in adapters)
            {
                var properties = adapter.GetIPProperties();
                foreach (var uniCast in properties.UnicastAddresses)
                {
                    if (!IPAddress.IsLoopback(uniCast.Address))
                        addresses.Add(uniCast.Address);
                }
            }
            return addresses;
        }

        private bool Connect()
        {
            var host = GetHost();
            if (host == null)
                return false;
            _managerIPEndPoint = new IPEndPoint(IPAddress.Parse(host), 1005);
            return true;
        }

        private string GetHost()
        {
            foreach (var host in _host)
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

        public IPEndPoint GetIPEndPoint()
        {
            return _managerIPEndPoint;
        }

        public IChannelPipe GetChannelPipe()
        {
            return this;
        }

        public void OnStartEvent(Netronics.Netronics netronics, EventArgs eventArgs)
        {
        }

        public void OnStopEvent(Netronics.Netronics netronics, EventArgs eventArgs)
        {
        }

        public IChannel CreateChannel(Netronics.Netronics netronics, Socket socket)
        {
            return null;
        }

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
            return null;
        }

        public IPacketDecoder GetDecoder()
        {
            return null;
        }
    }
}
