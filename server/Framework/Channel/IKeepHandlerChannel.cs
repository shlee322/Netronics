namespace Netronics.Channel
{
    /// <summary>
    /// <see cref="IChannelHandler"/>를 가지고 있는 <see cref="IChannel"/>
    /// </summary>
    interface IKeepHandlerChannel
    {
        /// <summary>
        /// <see cref="IChannel"/>에서 사용할 <see cref="IChannelHandler"/>를 설정
        /// </summary>
        /// <param name="handler"><see cref="IChannel"/>에서 사용할 <see cref="IChannelHandler"/></param>
        /// <returns>입력값을 그대로 리턴</returns>
        IChannelHandler SetHandler(IChannelHandler handler);
    }
}
