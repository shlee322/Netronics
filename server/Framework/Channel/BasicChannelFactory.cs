using System;
using System.Net.Sockets;
using Netronics.PacketEncoder;
using Netronics.PacketEncoder.Bson;
using Netronics.Protocol;

namespace Netronics.Channel
{
    public class BasicChannelFactory : IChannelFactory
    {
        private Func<IProtocol> _procotol = () => null;
        private Func<IChannelHandler> _handler = () => new BasicChannelHandler(); 

        public Channel CreateChannel(Netronics netronics, Socket socket)
        {
            return Channel.CreateChannel(socket, CreateFlag());
        }

        private ChannelFlag CreateFlag()
        {
            ChannelFlag flag = new ChannelFlag();
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
