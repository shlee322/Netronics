using System.Net.Sockets;
using Netronics.PacketEncoder.Bson;

namespace Netronics.Channel
{
    class BasicChannelFactory : IChannelFactory
    {
        public Channel NewChannel(Netronics netronics, Socket socket)
        {
            return Channel.NewChannel(socket, new BsonEncoder(), new BsonDecoder(), new BasicChannelHandler());
        }
    }
}
