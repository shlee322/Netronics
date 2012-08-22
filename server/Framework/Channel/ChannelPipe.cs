using System;
using System.Net.Sockets;
using Netronics.Channel.Channel;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;

namespace Netronics.Channel
{
    /// <summary>
    /// <see cref="SetProtocol"/>과 <see cref="SetHandler"/>를 사용할 수 있는 <see cref="IChannelPipe"/>
    /// </summary>
    public class ChannelPipe : IChannelPipe
    {
        private Func<IChannelHandler> _handler = () => new ChannelHandler();
        private Func<IPacketEncoder> _encoder = () => null;
        private Func<IPacketDecoder> _decoder = () => null;

        #region IChannelPipe Members

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            SocketChannel channel = SocketChannel.CreateChannel(socket);
            channel.SetConfig("encoder", _encoder());
            channel.SetConfig("decoder", _decoder());
            channel.SetConfig("handler", _handler());
            //channel.SetProtocol(_procotol());
            //channel.SetHandler(_handler());
            return channel;
        }

        #endregion

        /// <summary>
        /// <see cref="CreateChannel"/>에 의하여 생성되는 <see cref="IChannel"/>에서 사용할 <see cref="IProtocol"/>를 정의
        /// </summary>
        /// 
        /// <example>
        /// <code>
        ///     ChannelPipe factory = new ChannelPipe();
        ///     factory.SetProtocol(()=>new ExampleProtocol());
        /// </code>
        /// </example>
        /// 
        /// <param name="func"><see cref="IChannel"/>에서 사용할 <see cref="IProtocol"/>를 가져오는 Func</param>
        /// <returns>자신의 인스턴스</returns>
        public ChannelPipe SetEncoder(Func<IPacketEncoder> func)
        {
            _encoder = func;
            return this;
        }

        public ChannelPipe SetDecoder(Func<IPacketDecoder> func)
        {
            _decoder = func;
            return this;
        }

        /// <summary>
        /// <see cref="CreateChannel"/>에 의하여 생성되는 <see cref="IChannel"/>에서 사용할 <see cref="IChannelHandler"/>를 정의
        /// </summary>
        /// 
        /// <example>
        /// <code>
        ///     ChannelPipe factory = new ChannelPipe();
        ///     factory.SetHandler(()=>new ExampleHandler());
        /// </code>
        /// </example>
        /// 
        /// <param name="func"><see cref="IChannel"/>에서 사용할 <see cref="IChannelHandler"/>를 가져오는 Func</param>
        /// <returns>자신의 인스턴스</returns>
        public ChannelPipe SetHandler(Func<IChannelHandler> func)
        {
            _handler = func;
            return this;
        }
    }
}