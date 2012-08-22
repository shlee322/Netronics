using Netronics.Channel.Channel;

namespace Netronics.Channel
{
    public interface IReceiveContext
    {
        IChannel GetChannel();
        object GetMessage();
    }
}
