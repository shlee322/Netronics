using System;
using System.Net;
using System.Net.Sockets;
using Netronics;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Bson;
using Netronics.Protocol.PacketEncryptor;
using Netronics.Template.Http.Handler;

namespace Service.Manager
{
    class ServiceManager : IProperties, IChannelPipe, IProtocol
    {
        private static ServiceManager _manager;

        private static readonly BsonEncoder Encoder = new BsonEncoder();
        private static readonly BsonDecoder Decoder = new BsonDecoder();

        private readonly Netronics.Netronics _netronics;

        public static void Load(string name)
        {
            _manager = new ServiceManager();
        }

        public ServiceManager()
        {
            _netronics = new Netronics.Netronics(this);
            _netronics.Start();
        }

        public void OnStartEvent(Netronics.Netronics netronics, EventArgs eventArgs)
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

        public IChannel CreateChannel(Netronics.Netronics netronics, Socket socket)
        {
            var channel = SocketChannel.CreateChannel(socket);
            channel.SetProtocol(this);
            channel.SetHandler(new Handler());
            return channel;
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
            return Encoder;
        }

        public IPacketDecoder GetDecoder()
        {
            return Decoder;
        }
    }
}
