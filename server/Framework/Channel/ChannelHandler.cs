namespace Netronics.Channel
{
    internal class ChannelHandler : IChannelHandler
    {
        #region IChannelHandler Members

        public virtual void Connected(IChannel channel)
        {
        }

        public virtual void Disconnected(IChannel channel)
        {
        }

        public virtual void MessageReceive(IChannel channel, dynamic message)
        {
        }

        #endregion
    }
}