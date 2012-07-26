using Netronics;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;
using Service.Manager.Packets;

namespace Service.Manager
{
    class PacketEncoder : IPacketEncoder, IPacketDecoder
    {
        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            if(data is Ping)
            {
                var packet = new PacketBuffer();
                packet.WriteByte(0);
                return packet;
            }

            if(data is JoinService)
            {
                var packet = new PacketBuffer();
                packet.WriteByte(1);
                return packet;
            }
            return null;
        }

        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();
            if (buffer.AvailableBytes() < 1)
                return null;

            byte type = buffer.ReadByte();
            if(type == 0)
            {
                buffer.EndBufferIndex();
                return new Ping();
            }
            return null;
        }
    }
}
