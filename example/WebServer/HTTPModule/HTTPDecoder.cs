using System;
using System.Text.RegularExpressions;
using Netronics;
namespace HTTPModule
{
    class HTTPDecoder : IPacketDecoder
    {
        public dynamic Decode(PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();
            string data = System.Text.Encoding.UTF8.GetString(buffer.GetBytes());
            int headerendindex = data.LastIndexOf("\r\n\r\n", System.StringComparison.Ordinal);
            if (headerendindex == -1)
            {
                buffer.ResetBufferIndex();
                return null;
            }

            string[] header = Regex.Split(data, "\r\n");

            Request request = new Request();

            for (int i = 1; i < header.Length; i++ )
            {
                int index = header[i].IndexOf(':');
                if(index == -1)
                    continue;
                request.SetHeader(header[i].Substring(0, index), header[i].Substring(index)+1);
            }

            return request;
        }
    }
}
