using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class WebSocketDecoder : IPacketDecoder
    {
        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();
            if (buffer.AvailableBytes() < 3)
                return null;
            byte frameH = buffer.ReadByte();
            byte frameP = buffer.ReadByte();
            int len = frameP & 0x7F;
            if (len > 0x7D)
            {
                if (buffer.AvailableBytes() < 2)
                    return null;
                len = (len << 8) + buffer.ReadByte();
                if ((frameP & 0x7F) == 0x7F)
                {
                    if (buffer.AvailableBytes() < 2)
                        return null;
                    len = (len << 8) + buffer.ReadByte();
                }
            }

            if (buffer.AvailableBytes() < 4 + len)
                return null;

            byte[] key = (frameP & 0x80) == 0x80 ? buffer.ReadBytes(4) : null;

            var data = new byte[len];
            if (key == null)
            {
                data = buffer.ReadBytes(len);
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    data[i] = (byte)(buffer.ReadByte() ^ key[i % 4]);
                }
            }
            buffer.EndBufferIndex();
            return data;
        }
    }
}
