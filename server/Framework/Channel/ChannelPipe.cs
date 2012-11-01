using System;
using System.Net.Sockets;
using Netronics.Channel.Channel;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Linefeed;

namespace Netronics.Channel
{
    /// <summary>
    /// <see cref="SetProtocol"/>과 <see cref="SetHandler"/>를 사용할 수 있는 <see cref="IChannelPipe"/>
    /// </summary>
    public class ChannelPipe : IChannelPipe
    {
        private Action<SocketChannel> _createChannel;

        #region IChannelPipe Members

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            SocketChannel channel = SocketChannel.CreateChannel(socket);
            _createChannel(channel);
            return channel;
        }

        #endregion

        public ChannelPipe SetCreateChannelAction(Action<SocketChannel> action)
        {
            _createChannel = action;
            return this;
        }

        public static ChannelPipe CreateChannelPipe(IPacketEncoder encoder, IPacketDecoder decoder, IChannelHandler handler)
        {
            var pipe = new ChannelPipe();
            pipe.SetCreateChannelAction((channel) =>
            {
                channel.SetConfig("encoder", encoder);
                channel.SetConfig("decoder", decoder);
                channel.SetConfig("handler", handler);
            });
            return pipe;
        }

        public static ChannelPipe CreateChannelPipe(IChannelHandler handler)
        {
            return CreateChannelPipe(LinefeedEncoder.Encoder, LinefeedEncoder.Encoder, handler);
        }
    }
}