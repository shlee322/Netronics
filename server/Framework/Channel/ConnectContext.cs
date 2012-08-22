using Netronics.Channel.Channel;

namespace Netronics.Channel
{
    public class ConnectContext : IReceiveContext
    {
        private readonly IChannel _channel;

        public ConnectContext(IChannel channel)
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
