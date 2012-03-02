using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;

namespace Netronics.Template.Service.Protocol
{
    /// <summary>
    /// 기본적인 패킷 구조는 다음과 같다.
    /// 
    /// 요청 : 패킷타입(1byte), 요청서비스ID(4byte), 응답서비스ID(4byte), 트랜젝션ID(8byte), 메시지 클래스 이름 길이(2byte), 메시지 클래스 이름(메시지 클래스 이름 길이 byte), data길이(4byte), data
    /// 응답 : 패킷타입(1byte), 응답서비스ID(4byte), 요청서비스ID(4byte), 트랜젝션ID(8byte), 결과 클래스 이름 길이(2byte), 결과 클래스 이름(결과 클래스 이름 길이 byte), data길이(4byte), data
    /// 
    /// 패킷타입
    /// 
    /// 00000000 : 요청 (결과 수신)
    /// 00000001 : 요청 (결과 수신 안함)
    /// 00000010 : 결과 (성공)
    /// 00000011 : 결과 (실패)
    /// 00000100 : ID 알림
    /// </summary>
    class PacketEncoder : IPacketEncoder, IPacketDecoder
    {
        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
        }

        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();

            if (buffer.AvailableBytes() < 1)
                return null;

            byte type = buffer.ReadByte();



            buffer.EndBufferIndex();
            return null;
        }
    }
}
