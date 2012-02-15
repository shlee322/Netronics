using System;
using System.Net.Sockets;
using Netronics.Protocol;

namespace Netronics.Channel
{
    public class ChannelFactory : IChannelFactory
    {
        private Func<IChannelHandler> _handler = () => new ChannelHandler();
        private Func<IProtocol> _procotol = () => null;

        #region IChannelFactory Members

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            return SocketChannel.CreateChannel(socket);
        }

        #endregion

        public ChannelFactory SetProtocol(Func<IProtocol> func)
        {
            _procotol = func;
            return this;
        }

        public ChannelFactory SetHandler(Func<IChannelHandler> func)
        {
            _handler = func;
            return this;
        }
    }
}