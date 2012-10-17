using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Bson;

namespace Netronics.Mobile
{
    class ChannelPipe : IChannelPipe
    {
        private readonly Mobile _mobile;
        private readonly X509Certificate _cert;

        public ChannelPipe(Mobile mobile, X509Certificate cert)
        {
            _mobile = mobile;
            _cert = cert;
        }

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            var channel = SslChannel.CreateChannel(socket, _cert);
            channel.SetConfig("encoder", BsonEncoder.Encoder);
            channel.SetConfig("decoder", BsonDecoder.Decoder);
            channel.SetConfig("handler", new Handler(channel, _mobile));
            return channel;
        }
    }
}
