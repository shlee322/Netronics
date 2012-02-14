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
            return SocketChannel.CreateChannel(socket, CreateFlag());
        }

        #endregion

        private ChannelFlag CreateFlag()
        {
            var flag = new ChannelFlag();
            flag[ChannelFlag.Flag.Protocol] = _procotol();
            flag[ChannelFlag.Flag.Handler] = _handler();
            return flag;
        }


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