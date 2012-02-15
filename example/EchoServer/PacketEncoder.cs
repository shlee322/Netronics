using Netronics;
using Netronics.Channel;
using Netronics.PacketEncoder;
using Netronics.Protocol.PacketEncoder;

namespace EchoServer
{
    class PacketEncoder : IPacketEncoder
    {
        public PacketBuffer Encode(IChannel channel, dynamic data)
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
