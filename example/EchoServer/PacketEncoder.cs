using Netronics;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;

namespace EchoServer
{
    class PacketEncoder : IPacketEncoder
    {
        public PacketBuffer Encode(IChannel channel, object data)
        {
            if (data.GetType() != typeof(byte[]))
                return null;

            var bytes = (byte[]) data;
            var buffer = new PacketBuffer();
            buffer.Write(bytes, 0, bytes.Length);
            return buffer;
        }
    }
}
