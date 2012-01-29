namespace Netronics.Channel
{
    class BasicChannelHandler : IChannelHandler
    {
        public virtual void Connected(Channel channel)
        {
        }

        public virtual void Disconnected(Channel channel)
        {
        }

        public virtual void MessageReceive(Channel channel, dynamic message)
        {
        }
    }
}
