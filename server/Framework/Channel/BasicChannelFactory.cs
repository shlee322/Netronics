using System.Net.Sockets;
using Netronics.PacketEncoder.Bson;

namespace Netronics.Channel
{
    class BasicChannelFactory : IChannelFactory
    {
        public Channel CreateChannel(Netronics netronics, Socket socket)
        {
            return Channel.CreateChannel(socket, new BsonEncoder(), new BsonDecoder(), new BasicChannelHandler());
        }
    }
}
