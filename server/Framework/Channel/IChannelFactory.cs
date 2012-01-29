using System.Net.Sockets;

namespace Netronics.Channel
{
    public interface IChannelFactory
    {
        Channel NewChannel(Netronics netronics, Socket socket);
    }
}
