using System;
using System.Net.Sockets;
using Netronics.Protocol;

namespace Netronics.Channel
{
    public class BasicChannelFactory : IChannelFactory
    {
        private Func<IChannelHandler> _handler = () => new BasicChannelHandler();
        private Func<IProtocol> _procotol = () => null;

        #region IChannelFactory Members

        public Channel CreateChannel(Netronics netronics, Socket socket)
        {
            return Channel.CreateChannel(socket, CreateFlag());
        }

        #endregion

        private ChannelFlag CreateFlag()
        {
            var flag = new ChannelFlag();
            flag[ChannelFlag.Flag.Protocol] = _procotol();
            flag[ChannelFlag.Flag.Handler] = _handler();
            return flag;
        }


        public BasicChannelFactory SetProtocol(Func<IProtocol> func)
        {
            _procotol = func;
            return this;
        }

        public BasicChannelFactory SetHandler(Func<IChannelHandler> func)
        {
            _handler = func;
            return this;
        }
    }
}