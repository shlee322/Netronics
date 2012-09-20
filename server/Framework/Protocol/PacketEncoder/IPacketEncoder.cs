using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder
{
    public interface IPacketEncoder
    {
        /// <summary>
        /// 패킷을 인코딩하는 메서드
        /// </summary>
        /// <param name="channel">인코딩을 요청한 Channel</param>
        /// <param name="data">패킷 메시지 객체</param>
        /// <returns>PacketBuffer</returns>
        PacketBuffer Encode(IChannel channel, object data);
    }
}