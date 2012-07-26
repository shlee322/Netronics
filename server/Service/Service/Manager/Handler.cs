using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Service.Service.Manager
{
    class Handler : IChannelHandler
    {
        private ManagerProcessor _processor;

        public Handler(ManagerProcessor processor)
        {
            _processor = processor;
        }

        public void Connected(IChannel channel)
        {
            channel.SendMessage(_processor.GetJoinServicePacket());
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
        }
    }
}
