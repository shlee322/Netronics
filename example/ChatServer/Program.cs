using System.Net;
using System.Threading;
using Netronics;
using Netronics.Channel;
using Netronics.Protocol;

namespace ChatServer
{
    class Program
    {
        private static readonly Handler Handler = new Handler();

        static void Main(string[] args)
        {
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Any, 7777),
                ChannelPipe.CreateChannelPipe(Handler));
            var netronics = new Netronics.Netronics(properties);
            netronics.Start();
        }
    }
}
