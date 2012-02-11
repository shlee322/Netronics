using System.Text.RegularExpressions;

namespace Netronics.PacketEncoder.Http
{
    public class HttpDecoder : IPacketDecoder
    {
        public dynamic Decode(Channel.Channel channel, PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();
            string data = System.Text.Encoding.UTF8.GetString(buffer.GetBytes());
            int headerendindex = data.IndexOf("\r\n\r\n", System.StringComparison.Ordinal);
            if (headerendindex == -1)
            {
                buffer.ResetBufferIndex();
                return null;
            }

            buffer.SetPosition(headerendindex+4);
            buffer.EndBufferIndex();

            data = data.Substring(0, headerendindex);
            
            string[] header = Regex.Split(data, "\r\n");

            Request request = new Request();

            for (int i = 1; i < header.Length; i++ )
            {
                int index = header[i].IndexOf(':');
                if(index == -1)
                    continue;
                request.SetHeader(header[i].Substring(0, index), header[i].Substring(index+1));
            }

            return request;
        }
    }
}
