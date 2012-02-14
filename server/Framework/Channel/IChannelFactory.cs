using System.Net.Sockets;

namespace Netronics.Channel
{
    public interface IChannelFactory
    {
        IChannel CreateChannel(Netronics netronics, Socket socket);
    }
}