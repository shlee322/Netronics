using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class HttpDecoder : IPacketDecoder
    {
        #region IPacketDecoder Members

        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();
            TextReader reader = new StreamReader(buffer.GetStream());
            Request request = Request.GetRequest(reader);
            if (request == null)
            {
                buffer.ResetBufferIndex();
                return null;
            }
            return request;
        }

        #endregion
    }
}