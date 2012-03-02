using System.Net;
using System.Threading;
using Netronics;
using Netronics.Channel;
using Netronics.Protocol;

namespace BroadcastServer
{
    class Program
    {
        private static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);
        private static readonly Handler Handler = new Handler();

        static void Main(string[] args)
        {
            var properties = new Properties();
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 7777));
            properties.SetChannelFactoryOption(factory => SetFatoryOption((ChannelFactory)factory));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();

            ExitEvent.WaitOne();
        }

        private static void SetFatoryOption(ChannelFactory factory)
        {
            PacketEncoder encoder = new PacketEncoder();
            factory.SetProtocol(() => new ModifiableProtocol(encoder: encoder, decoder: encoder));
            factory.SetHandler(() => Handler);
        }
    }
}
