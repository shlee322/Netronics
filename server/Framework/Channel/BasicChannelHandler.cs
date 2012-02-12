namespace Netronics.Channel
{
    internal class BasicChannelHandler : IChannelHandler
    {
        #region IChannelHandler Members

        public virtual void Connected(Channel channel)
        {
        }

        public virtual void Disconnected(Channel channel)
        {
        }

        public virtual void MessageReceive(Channel channel, dynamic message)
        {
        }

        #endregion
    }
}