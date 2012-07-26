using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Http;
using Netronics.Protocol.PacketEncryptor;

namespace Netronics.Template.Http
{
    public class HttpsProperties : IProperties, IProtocol, IChannelPipe
    {
        private static readonly HttpEncoder Encoder = new HttpEncoder();
        private static readonly HttpDecoder Decoder = new HttpDecoder();
        private readonly Func<IChannelHandler> _handler;
        protected IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, 0);
       

        public HttpsProperties(Func<IChannelHandler> handler, int port = 443)
        {
            IpEndPoint = new IPEndPoint(IPAddress.Any, port);
            _handler = handler;
        }

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            SslChannel channel = SslChannel.CreateChannel(socket, X509Certificate.CreateFromCertFile("test.cert"));
            channel.SetProtocol(this);
            channel.SetHandler(_handler());
            return channel;
        }

        public IPacketEncryptor GetEncryptor()
        {
            return null;
        }

        public IPacketDecryptor GetDecryptor()
        {
            return null;
        }

        public IPacketEncoder GetEncoder()
        {
            return Encoder;
        }

        public IPacketDecoder GetDecoder()
        {
            return Decoder;
        }


        public void OnStartEvent(Netronics netronics, EventArgs eventArgs)
        {
        }

        public void OnStopEvent(Netronics netronics, EventArgs eventArgs)
        {
        }

        public IPEndPoint GetIPEndPoint()
        {
            return IpEndPoint;
        }

        public IChannelPipe GetChannelPipe()
        {
            return this;
        }
    }
}
