using Netronics.Protocol;

namespace Netronics.Channel.Channel
{
    interface IKeepProtocolChannel : IChannel
    {
        /// <summary>
        /// <see cref="IChannel"/>에서 사용하는 <see cref="IProtocol"/>을 받아옴
        /// </summary>
        /// <returns><see cref="IChannel"/>에서 사용하는 <see cref="IProtocol"/></returns>
        IProtocol GetProtocol();

        IProtocol SetProtocol(IProtocol protocol);
    }
}
