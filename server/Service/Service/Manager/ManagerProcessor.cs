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
using Netronics.Protocol.PacketEncoder.Bson;
using Netronics.Protocol.PacketEncryptor;
using Newtonsoft.Json.Linq;
using Service.Service.Loader;

namespace Service.Service.Manager
{
    internal class ManagerProcessor : IProperties, IChannelPipe, IProtocol
    {
        private static readonly BsonEncoder Encoder = new BsonEncoder();
        private static readonly BsonDecoder Decoder = new BsonDecoder();
        private readonly string[] _host;

        private readonly Netronics.Netronics _manager;
        private ServiceLoader _loader;

        private IPEndPoint _managerIPEndPoint;

        private uint _serviceId = 0;


        public ManagerProcessor(ServiceLoader loader, string[] host)
        {
            _loader = loader;
            _host = host;
            _manager = new Client(this);

            Connect();
            _manager.Start();
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
            packet.type = "joinService";
            if (_serviceId != 0)
                packet.id = _serviceId;
            packet.name = _loader.GetServiceName();
            packet.address = new JArray(GetIPAddress());
            return packet;
        }
    }
}