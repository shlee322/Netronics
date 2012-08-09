using System;
using System.Net;
using System.Net.Sockets;
using Netronics;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Event;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Bson;
using Netronics.Protocol.PacketEncryptor;

namespace Service.Service.Manager.Properties
{
    class Properties : IProperties, IProtocol, IChannelPipe
    {
        private static readonly BsonEncoder Encoder = new BsonEncoder();
        private static readonly BsonDecoder Decoder = new BsonDecoder();

        private readonly IPEndPoint _ipEndPoint;
        private readonly Func<IChannelHandler> _func;

        public Properties(IPEndPoint ipEndPoint, Func<IChannelHandler> func)
        {
            _ipEndPoint = ipEndPoint;
            _func = func;
        }

        public void OnStartEvent(Netronics.Netronics netronics, StartEventArgs eventArgs)
        {
        }

        public void OnStopEvent(Netronics.Netronics netronics, EventArgs eventArgs)
        {
        }

        public IPEndPoint GetIPEndPoint()
        {
            return _ipEndPoint;
        }

        public IChannelPipe GetChannelPipe()
        {
            return this;
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

        public IChannel CreateChannel(Netronics.Netronics netronics, Socket socket)
        {
            SocketChannel channel = SocketChannel.CreateChannel(socket);
            channel.SetProtocol(this);
            channel.SetHandler(_func());
            return channel;
        }
    }
}
