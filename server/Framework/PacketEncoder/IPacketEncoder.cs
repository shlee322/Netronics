namespace Netronics.PacketEncoder
{
    public interface IPacketEncoder
    {
        /// <summary>
        /// 패킷을 인코딩하는 메서드
        /// </summary>
        /// <param name="data">패킷 메시지 객체</param>
        /// <returns>PacketBuffer</returns>
        PacketBuffer Encode(hannel.Channel channel, dynamic data);
    }
}