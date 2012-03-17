using System.Text;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class HttpEncoder : IPacketEncoder
    {
        #region IPacketEncoder Members

        public PacketBuffer Encode(IChannel channel, dynamic message)
        {
            var buffer = new PacketBuffer();

            string content = message.GetContent();
            if (content == null)
                content = "";
            string data = "HTTP/1.0 200 OK\nContent-Type: text/html\nContent-Length: " + content.Length + "\n\n" +
                          content;
            byte[] packet = Encoding.UTF8.GetBytes(data);
            buffer.Write(packet, 0, packet.Length);
            return buffer;
        }

        #endregion
    }
}