using System.IO;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.WebSocket
{
    public class Decoder : IPacketDecoder
    {
        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();
            var stream = new MemoryStream();
            while (true)
            {
                if (buffer.AvailableBytes() < 2)
                    return null;
                byte frameH = buffer.ReadByte();
                byte frameP = buffer.ReadByte();
                int len = frameP & 0x7F;
                if (len > 0x7D)
                {
                    if (buffer.AvailableBytes() < 2)
                        return null;

                    for (var i = 0; i < 2; i++)
                        len = (len << 8) + buffer.ReadByte();
                    
                    if ((frameP & 0x7F) == 0x7F)
                    {
                        if (buffer.AvailableBytes() < 2)
                            return null;
                        for (var i = 0; i < 2; i++)
                            len = (len << 8) + buffer.ReadByte();
                    }
                }

                if (buffer.AvailableBytes() < 4 + len)
                    return null;

                byte[] key = (frameP & 0x80) == 0x80 ? buffer.ReadBytes(4) : null;

                byte[] data = null;
                if (key == null)
                {
                    data = buffer.ReadBytes(len);
                }
                else
                {
                    data = new byte[len];
                    for (int i = 0; i < len; i++)
                        data[i] = (byte) (buffer.ReadByte() ^ key[i%4]);
                }
                stream.Write(data, 0, len);

                if ((frameH & 0xF) == 8)
                {
                    buffer.EndBufferIndex();
                    return new ConnectionClose();
                }
                if ((frameH & 0xF) == 9)
                {
                    buffer.EndBufferIndex();
                    return new Ping();
                }
                if((frameH & 0xF) == 10)
                {
                    buffer.EndBufferIndex();
                    return new Pong();
                }

                if((frameH & 0x80) == 128)
                    break;
            }
            buffer.EndBufferIndex();
            return stream.ToArray();
        }
    }
}
