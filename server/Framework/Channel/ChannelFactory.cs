using System;
using System.Net.Sockets;
using Netronics.Protocol;

namespace Netronics.Channel
{
    /// <summary>
    /// <see cref="SetProtocol"/>과 <see cref="SetHandler"/>를 사용할 수 있는 <see cref="IChannelFactory"/>
    /// </summary>
    public class ChannelFactory : IChannelFactory
    {
        private Func<IChannelHandler> _handler = () => new ChannelHandler();
        private Func<IProtocol> _procotol = () => null;

        #region IChannelFactory Members

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            SocketChannel channel = SocketChannel.CreateChannel(socket);
            channel.SetProtocol(_procotol());
            channel.SetHandler(_handler());
            return channel;
        }

        #endregion

        /// <summary>
        /// <see cref="CreateChannel"/>에 의하여 생성되는 <see cref="IChannel"/>에서 사용할 <see cref="IProtocol"/>를 정의
        /// </summary>
        /// 
        /// <example>
        /// <code>
        ///     ChannelFactory factory = new ChannelFactory();
        ///     factory.SetProtocol(()=>new ExampleProtocol());
        /// </code>
        /// </example>
        /// 
        /// <param name="func"><see cref="IChannel"/>에서 사용할 <see cref="IProtocol"/>를 가져오는 Func</param>
        /// <returns>자신의 인스턴스</returns>
        public ChannelFactory SetProtocol(Func<IProtocol> func)
        {
            _procotol = func;
            return this;
        }

        /// <summary>
        /// <see cref="CreateChannel"/>에 의하여 생성되는 <see cref="IChannel"/>에서 사용할 <see cref="IChannelHandler"/>를 정의
        /// </summary>
        /// 
        /// <example>
        /// <code>
        ///     ChannelFactory factory = new ChannelFactory();
        ///     factory.SetHandler(()=>new ExampleHandler());
        /// </code>
        /// </example>
        /// 
        /// <param name="func"><see cref="IChannel"/>에서 사용할 <see cref="IChannelHandler"/>를 가져오는 Func</param>
        /// <returns>자신의 인스턴스</returns>
        public ChannelFactory SetHandler(Func<IChannelHandler> func)
        {
            _handler = func;
            return this;
        }
    }
}