using Netronics.Channel.Channel;

namespace Netronics.Channel
{
    internal class ChannelHandler : IChannelHandler
    {
        #region IChannelHandler Members

        public virtual void Connected(IReceiveContext context)
        {
        }

        public virtual void Disconnected(IReceiveContext context)
        {
        }

        public virtual void MessageReceive(IReceiveContext context)
        {
        }

        #endregion
    }
}