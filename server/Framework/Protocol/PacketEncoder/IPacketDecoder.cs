using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder
{
    public interface IPacketDecoder
    {
        /// <summary>
        /// 패킷을 디코딩하는 메서드
        /// </summary>
        /// <param name="channel">디코딩을 요청한 Channel</param>
        /// <param name="buffer">PacketBuffer</param>
        /// <returns>패킷 메시지 객체</returns>
        object Decode(IChannel channel, PacketBuffer buffer);
    }
}