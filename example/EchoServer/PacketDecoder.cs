using Netronics;
using Netronics.PacketEncoder;

namespace EchoServer
{
    class PacketDecoder : IPacketDecoder
    {
        public dynamic Decode(PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();
            if (buffer.LegibleBytes() < 1)
            {
                buffer.ResetBufferIndex();
                return null;
            }

            var data = new byte[buffer.LegibleBytes()];
            buffer.ReadBytes(data);

            buffer.EndBufferIndex();

            return data;
        }
    }
}
