using Netronics.Channel;

namespace Netronics.Protocol.PacketEncoder
{
    public interface IPacketDecoder
    {
        /// <summary>
        /// 패킷을 디코딩하는 메서드
        /// </summary>
        /// <param name="buffer">PacketBuffer</param>
        /// <returns>패킷 메시지 객체</returns>
        dynamic Decode(IChannel channel, PacketBuffer buffer);
    }
}