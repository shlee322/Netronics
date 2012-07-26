using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Service.Manager
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
            if(message.type == "")
            {
            }
        }
    }
}
