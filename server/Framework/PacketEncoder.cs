namespace Netronics
{
    public interface PacketEncoder
    {
        /// <summary>
        /// 패킷을 인코딩하는 메서드
        /// </summary>
        /// <param name="buffer">패킷 메시지 객체</param>
        /// <returns>PacketBuffer</returns>
        PacketBuffer encode(dynamic data);
    }
}