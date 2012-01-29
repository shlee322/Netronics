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

        public Channel.Channel NewChannel(Netronics netronics, Socket socket)
        {
            return Channel.Channel.NewChannel(socket, new BsonEncoder(), new BsonDecoder(), new RemoteService(_manager));
        }
    }
}
