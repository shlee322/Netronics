using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Event;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Template.Http
{
    public class HttpsProperties : IProperties, IChannelPipe
    {
        private static readonly HttpEncoder Encoder = new HttpEncoder();
        private static readonly HttpDecoder Decoder = new HttpDecoder();
        private readonly Func<IChannelHandler> _handler;
        protected IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        private string _certFile;
       

        public HttpsProperties(Func<IChannelHandler> handler, string certFile, int port = 443)
        {
            IpEndPoint = new IPEndPoint(IPAddress.Any, port);
            _handler = handler;
            _certFile = certFile;
        }

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            SslChannel channel = SslChannel.CreateChannel(socket, X509Certificate.CreateFromCertFile("test.cert"));
            channel.SetConfig("encoder", Encoder);
            channel.SetConfig("decoder", Decoder);
            channel.SetConfig("handler", _handler());
            return channel;
        }

        public void OnStartEvent(Netronics netronics, StartEventArgs eventArgs)
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
