namespace Netronics.Channel
{
    public interface IChannelHandler
    {
        /// <summary>
        /// 채널과 연결될 경우 호출되는 메서드
        /// </summary>
        /// <param name="channel">연결된 채널</param>
        void Connected(IChannel channel);
        /// <summary>
        /// 채널과 연결이 끈길 경우 호출되는 메서드
        /// </summary>
        /// <param name="channel">연결이 끈긴 채널</param>
        void Disconnected(IChannel channel);

        /// <summary>
        /// 채널로부터 메시지를 수신할 경우 호출되는 메서드
        /// </summary>
        /// <param name="channel">메시지를 전송한 채널</param>
        /// <param name="message">메시지</param>
        void MessageReceive(IChannel channel, dynamic message);
    }
}