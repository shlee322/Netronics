using System.Net.Sockets;
using Netronics.Channel;

namespace Netronics.Template.Service
{
    internal class ServiceChannelFactory : IChannelFactory
    {
        private readonly ServiceManager _manager;

        public ServiceChannelFactory(ServiceManager manager)
        {
            _manager = manager;
        }

        #region IChannelFactory Members

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            return SocketChannel.CreateChannel(socket, CreateFlag());
        }

        #endregion

        private ChannelFlag CreateFlag()
        {
            var flag = new ChannelFlag();
            //flag[ChannelFlag.Flag.Encoder] = new BsonEncoder();
            //flag[ChannelFlag.Flag.Decoder] = new BsonDecoder();
            flag[ChannelFlag.Flag.Handler] = new RemoteService(_manager);
            return flag;
        }
    }
}