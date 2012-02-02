using Netronics;
using Netronics.Channel;

namespace EchoServer
{
    class Handler : IChannelHandler
    {
        public void Connected(Channel channel)
        {
        }

        public void Disconnected(Channel channel)
        {
        }

        public void MessageReceive(Channel channel, dynamic message)
        {
            channel.SendMessage(message);
        }
    }
}
