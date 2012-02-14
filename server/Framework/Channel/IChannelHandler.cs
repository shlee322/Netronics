namespace Netronics.Channel
{
    public interface IChannelHandler
    {
        void Connected(IChannel channel);
        void Disconnected(IChannel channel);
        void MessageReceive(IChannel channel, dynamic message);
    }
}