using Netronics.Channel.Channel;

namespace Netronics.Channel
{
    class DisconnectContext : IReceiveContext
    {
        private IChannel _channel;

        public DisconnectContext(IChannel channel)
        {
            _channel = channel;
        }

        public IChannel GetChannel()
        {
            return _channel;
        }

        public object GetMessage()
        {
            return this;
        }
    }
}
