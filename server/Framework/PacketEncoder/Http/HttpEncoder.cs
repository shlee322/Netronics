namespace Netronics.PacketEncoder.Http
{
    public class HttpEncoder : IPacketEncoder
    {
        public PacketBuffer Encode(hannel.Channel channel, dynamic message)
        {
            PacketBuffer buffer = new PacketBuffer();

            string content = message.GetContent();
            if (content == null)
                content = "";
            string data = "HTTP/1.0 200 OK\r\nContent-Type: text/html\r\nContent-Length: " + content.Length + "\r\n\r\n" + content;
            byte[] packet = System.Text.Encoding.UTF8.GetBytes(data);
            buffer.Write(packet, 0, packet.Length);
            return buffer;
        }
    }
}
