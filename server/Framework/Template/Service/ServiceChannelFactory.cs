using System.Net.Sockets;
using Netronics.Channel;
using Netronics.PacketEncoder.Bson;

namespace Netronics.Template.Service
{
    class ServiceChannelFactory : IChannelFactory
    {
        private ServiceManager _manager;

        public ServiceChannelFactory(ServiceManager manager)
        {
            _manager = manager;
        }

        public Channel.Channel CreateChannel(Netronics netronics, Socket socket)
        {
            return Channel.Channel.CreateChannel(socket, CreateFlag());
        }

        private ChannelFlag CreateFlag()
        {
            ChannelFlag flag = new ChannelFlag();
            flag[ChannelFlag.Flag.Encoder] = new BsonEncoder();
            flag[ChannelFlag.Flag.Decoder] = new BsonDecoder();
            flag[ChannelFlag.Flag.Handler] = new RemoteService(_manager);
            return flag;
        }
    }
}
