using System.Net.Sockets;

namespace Netronics.Channel
{
    public interface IChannelFactory
    {
        Channel CreateChannel(Netronics netronics, Socket socket);
    }
}
