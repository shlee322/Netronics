using Netronics;
using Netronics.Channel;

namespace EchoServer
{
    class Handler : IChannelHandler
    {
        public void Connected(IChannel channel)
        {
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            channel.SendMessage(message);
        }
    }
}
