using System.IO;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class HttpDecoder : IPacketDecoder
    {
        public static int MaxBufferSize = 31457280;

        #region IPacketDecoder Members

        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();
            if(buffer.AvailableBytes() > MaxBufferSize)
            {
                channel.Disconnect();
                return null;
            }
            var reader = new StreamReader(buffer.GetStream());
            var request = Request.GetRequest(reader);
            if (request == null || (request.GetMethod() == "POST" && System.Convert.ToInt32(request.GetHeader("Content-Length")) > buffer.AvailableBytes()))
            {
                buffer.ResetBufferIndex();
                return null;
            }

            if (request.GetMethod() == "POST")
                request.SetPostData(new PostDataStream(request, buffer.GetStream()));
            else
                reader.ReadLine();

            buffer.EndBufferIndex();
            return request;
        }

        #endregion
    }
}