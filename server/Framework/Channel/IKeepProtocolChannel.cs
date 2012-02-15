using Netronics.Protocol;

namespace Netronics.Channel
{
    interface IKeepProtocolChannel
    {
        IProtocol SetProtocol(IProtocol protocol);
    }
}
