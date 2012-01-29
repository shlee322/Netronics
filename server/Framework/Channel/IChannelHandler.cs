namespace Netronics.Channel
{
    public interface IChannelHandler
    {
        void Connected(Channel channel);
        void Disconnected(Channel channel);
        void MessageReceive(Channel channel, dynamic message);
    }
}
