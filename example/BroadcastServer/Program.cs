using System.Net;
using System.Threading;
using Netronics;
using Netronics.Channel;

namespace BroadcastServer
{
    class Program
    {
        private static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);
        private static readonly Handler Handler = new Handler();

        static void Main(string[] args)
        {
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Any, 7777),
                                             new ChannelPipe().SetCreateChannelAction((channel) =>
                                             {
                                                 channel.SetConfig("encoder", PacketEncoder.Encoder);
                                                 channel.SetConfig("decoder", PacketEncoder.Encoder);
                                                 channel.SetConfig("handler", Handler);
                                             }));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();

            ExitEvent.WaitOne();
        }
    }
}
