using Netronics.Channel;
using Netronics.Channel.Channel;

namespace EchoServer
{
    class Handler : IChannelHandler
    {
        public void Connected(IReceiveContext channel)
        {
        }

        public void Disconnected(IReceiveContext channel)
        {
        }

        public void MessageReceive(IReceiveContext context)
        {
            context.GetChannel().SendMessage(context.GetMessage());
        }
    }
}
