using System;
using System.Net;
using System.Net.Sockets;
using Netronics;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Event;
using Netronics.Protocol.PacketEncoder.Bson;

namespace Service.Service.Manager
{
    class Properties : IProperties, IChannelPipe
    {
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

        public IChannel CreateChannel(Netronics.Netronics netronics, Socket socket)
        {
            SocketChannel channel = SocketChannel.CreateChannel(socket);
            channel.SetConfig("encoder", BsonEncoder.Encoder);
            channel.SetConfig("decode", BsonDecoder.Decoder);
            channel.SetConfig("handler", _func());
            return channel;
        }
    }
}
