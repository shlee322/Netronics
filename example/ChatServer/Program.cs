using System.Net;
using System.Threading;
using Netronics;
using Netronics.Channel;
using Netronics.Protocol;

namespace ChatServer
{
    class Program
    {
        private static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);
        private static readonly Handler Handler = new Handler();

        static void Main(string[] args)
        {
            var properties = new Properties();
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 7777));
            properties.SetChannelFactoryOption(factory => SetFactoryOption((ChannelPipe)factory));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();

            ExitEvent.WaitOne();
        }

        private static void SetFactoryOption(ChannelPipe pipe)
        {
            PacketEncoder encoder = new PacketEncoder();
            pipe.SetProtocol(() => new ModifiableProtocol(encoder: encoder, decoder: encoder));
            pipe.SetHandler(() => Handler);
        }
    }
}
