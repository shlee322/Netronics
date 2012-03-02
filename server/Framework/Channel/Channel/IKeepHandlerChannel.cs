using Netronics.Template.Service;

namespace Netronics.Channel.Channel
{
    /// <summary>
    /// <see cref="IChannelHandler"/>를 가지고 있는 <see cref="IChannel"/>
    /// </summary>
    interface IKeepHandlerChannel : IChannel
    {
        /// <summary>
        /// <see cref="IChannel"/>에서 사용하는 <see cref="IChannelHandler"/>을 받아옴
        /// </summary>
        /// <returns><see cref="IChannel"/>에서 사용하는 <see cref="IChannelHandler"/></returns>
        IChannelHandler GetHandler();

        /// <summary>
        /// <see cref="IChannel"/>에서 사용할 <see cref="IChannelHandler"/>를 설정
        /// </summary>
        /// <param name="handler"><see cref="IChannel"/>에서 사용할 <see cref="IChannelHandler"/></param>
        /// <returns>입력값을 그대로 리턴</returns>
        IChannelHandler SetHandler(IChannelHandler handler);
    }
}
