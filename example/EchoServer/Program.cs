using System.Net;
using System.Threading;
using Netronics.Channel;
using Netronics;
using Netronics.Protocol;

namespace EchoServer
{
    class Program
    {
        private static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Any, 7777),
                                                         new ChannelPipe().SetCreateChannelAction((channel) =>
                                                             {
                                                                 channel.SetConfig("encoder", new PacketEncoder());
                                                                 channel.SetConfig("decoder", new PacketDecoder());
                                                                 channel.SetConfig("handler", new Handler());
                                                             }));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();

            ExitEvent.WaitOne();
        }
    }
}