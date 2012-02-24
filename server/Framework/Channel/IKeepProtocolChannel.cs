using Netronics.Protocol;

namespace Netronics.Channel
{
    interface IKeepProtocolChannel : IChannel
    {
        IProtocol SetProtocol(IProtocol protocol);
    }
}
