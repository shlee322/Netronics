using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Bson;

namespace SslServer
{
    class ChannelPipe : IChannelPipe
    {
        private readonly X509Certificate _cert;

        public ChannelPipe(X509Certificate cert)
        {
            _cert = cert;
        }

        public IChannel CreateChannel(Netronics.Netronics netronics, Socket socket)
        {
            SslChannel channel;
            try
            {
                channel = SslChannel.CreateChannel(socket, _cert);
                channel.SetConfig("encoder", BsonEncoder.Encoder);
                channel.SetConfig("decoder", BsonDecoder.Decoder);
                channel.SetConfig("handler", new Handler());
            }
            catch(System.Exception e)
            {
                return null;
            }

            return channel;
        }
    }
}
