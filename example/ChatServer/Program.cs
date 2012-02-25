using System.Net;
using Netronics;
using Netronics.Protocol;

namespace ChatServer
{
    class Program
    {
        private static Handler Handler = new Handler();

        static void Main(string[] args)
        {
            var properties = new Properties();
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 7777));
            properties.SetChannelFactoryOption(factory => SetFatoryOption((ChannelFactory)factory));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();
        }

        private static void SetFatoryOption(ChannelFactory factory)
        {
            PacketEncoder encoder = new PacketEncoder();
            factory.SetProtocol(() => new ModifiableProtocol(encoder: encoder, decoder: encoder));
            factory.SetHandler(() => Handler);
        }
    }
}
