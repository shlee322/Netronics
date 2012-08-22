using Netronics.Channel.Channel;

namespace Netronics.Channel
{
    class MessageContext : IReceiveContext
    {
        private readonly IChannel _channel;
        private readonly object _message;

        public MessageContext(IChannel channel, object message)
        {
            _channel = channel;
            _message = message;
        }

        public IChannel GetChannel()
        {
            return _channel;
        }

        public object GetMessage()
        {
            return _message;
        }
    }
}
