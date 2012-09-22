using Netronics;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;

namespace EchoServer
{
    class PacketEncoder : IPacketEncoder, IPacketDecoder
    {
        public static PacketEncoder Encoder = new PacketEncoder();

        // SendMessage로 보낸 메시지를 PacketBuffer형태로 변환합니다.
        public PacketBuffer Encode(IChannel channel, object data)
        {
            if (data.GetType() != typeof(byte[])) // 보내는 메시지가 byte[]가 아닐 경우 전송 취소
                return null;

            var bytes = (byte[]) data;
            var buffer = new PacketBuffer();
            buffer.Write(bytes, 0, bytes.Length); // Buffer에 bytes를 쓴다.
            return buffer;
        }

        public object Decode(IChannel channel, PacketBuffer buffer)
        {
            var data = new byte[buffer.AvailableBytes()]; // 버퍼에 남아 있는 사이즈 만큼 byte 배열 할당
            buffer.ReadBytes(data); // 버퍼에서 byte[]를 읽어옴
            return data;
        }
    }
}
