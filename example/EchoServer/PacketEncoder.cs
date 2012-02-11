using Netronics;
using Netronics.Channel;
using Netronics.PacketEncoder;

namespace EchoServer
{
    class PacketEncoder : IPacketEncoder
    {
        public PacketBuffer Encode(Channel channel, dynamic data)
        {
            if (data.GetType() != typeof(byte[]))
                return null;

            byte[] bytes = (byte[]) data;
            PacketBuffer buffer = new PacketBuffer();
            buffer.Write(bytes, 0, bytes.Length);
            return buffer;
        }
    }
}
