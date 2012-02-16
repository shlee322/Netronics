namespace Netronics.Channel
{
    /// <summary>
    /// <see cref="Netronics"/>의 핵심 인터페이스이며 다른 클라이언트와 통신할 수 있는 통로이다.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// <see cref="IChannel"/>이 연결될 경우 호출되는 메서드
        /// </summary>
        void Connect();

        /// <summary>
        /// <see cref="IChannel"/>과 연결 끈어질 경우 호출되는 메서드
        /// </summary>
        void Disconnect();

        /// <summary>
        /// <see cref="IChannel"/>에게 메시지를 전송하는 메서드
        /// </summary>
        /// <param name="message"><see cref="IChannel"/>에게 전송할 메시지</param>
        void SendMessage(dynamic message);
    }
}