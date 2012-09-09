using Netronics;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;

namespace EchoServer
{
    class PacketEncoder : IPacketEncoder, IPacketDecoder
    {
        public static PacketEncoder Encoder = new PacketEncoder();

        public PacketBuffer Encode(IChannel channel, object data)
        {
            if (data.GetType() != typeof(byte[]))
                return null;

            var bytes = (byte[]) data;
            var buffer = new PacketBuffer();
            buffer.Write(bytes, 0, bytes.Length);
            return buffer;
        }

        public object Decode(IChannel channel, PacketBuffer buffer)
        {
            var data = new byte[buffer.AvailableBytes()];
            buffer.ReadBytes(data);
            return data;
        }
    }
}
