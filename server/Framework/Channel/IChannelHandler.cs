using Netronics.Channel.Channel;

namespace Netronics.Channel
{
    /// <summary>
    /// Netronics에서 Channel을 처리하는 인터페이스
    /// </summary>
    public interface IChannelHandler
    {
        /// <summary>
        /// 채널과 연결될 경우 호출되는 메서드
        /// </summary>
        /// <param name="context">Context</param>
        void Connected(IReceiveContext context);
        /// <summary>
        /// 채널과 연결이 끈길 경우 호출되는 메서드
        /// </summary>
        /// <param name="context">Context</param>
        void Disconnected(IReceiveContext context);

        /// <summary>
        /// 채널로부터 메시지를 수신할 경우 호출되는 메서드
        /// </summary>
        /// <param name="context">Context</param>
        void MessageReceive(IReceiveContext context);
    }
}