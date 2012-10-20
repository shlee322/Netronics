using System.IO;
using MsgPack;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.MsgPack
{
    public class MsgPackDecoder : IPacketDecoder
    {
        public static MsgPackDecoder Decoder = new MsgPackDecoder();

        public object Decode(IChannel channel, PacketBuffer buffer)
        {
            //버퍼 읽기 시작을 알림
            buffer.BeginBufferIndex();

            if (buffer.AvailableBytes() < 6) //버퍼길이가 5미만이면 리턴
                return null;
            buffer.ReadByte();
            uint len = buffer.ReadUInt32();
            if (len > buffer.AvailableBytes())
            {
                //버퍼의 길이가 실제 패킷 길이보다 모자름으로, 리셋후 리턴
                buffer.ResetBufferIndex();
                return null;
            }

            var data = new byte[len];
            buffer.ReadBytes(data);

            buffer.EndBufferIndex();

            var stream = new MemoryStream(data);
            var res = Unpacker.Create(stream).Unpack<MessagePackObject>();
            stream.Dispose();

            return res;
        }
    }
}
