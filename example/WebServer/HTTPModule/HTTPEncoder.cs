using Netronics;

namespace HTTPModule
{
    class HTTPEncoder : IPacketEncoder
    {
        public PacketBuffer Encode(dynamic message)
        {
            PacketBuffer buffer = new PacketBuffer();

            string content = message.GetContent();
            string data = "HTTP/1.1 200\r\nContent-Length:"+content.Length+"\r\n\r\n"+content;
            byte[] packet = System.Text.Encoding.UTF8.GetBytes(data);
            buffer.Write(packet, 0, packet.Length);
            return buffer;
        }
    }
}
