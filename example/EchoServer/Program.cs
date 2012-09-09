using System.Net;
using System.Threading;
using Netronics.Channel;
using Netronics;
using Netronics.Protocol;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Any, 7777),
                                                         new ChannelPipe().SetCreateChannelAction((channel) =>
                                                             {
                                                                 channel.SetConfig("encoder", PacketEncoder.Encoder);
                                                                 channel.SetConfig("decoder", PacketEncoder.Encoder);
                                                                 channel.SetConfig("handler", new Handler());
                                                             }));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();
        }
    }
}