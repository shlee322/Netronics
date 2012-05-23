using System;
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

            if (buffer.AvailableBytes() == 0)
                return null;

            Request request;

            try
            {
                request = Request.Parse(buffer);
            }
            catch (Exception)
            {
                buffer.ResetBufferIndex();
                return null;
            }

            buffer.EndBufferIndex();
            return request;
        }

        #endregion

    }
}