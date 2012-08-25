using System.Net;
using NUnit.Framework;
using Netronics;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Framework
{
    [TestFixture]
    public class NetronicsTest
    {
        [Test]
        public void Test1()
        {
            var netronics = new Netronics.Netronics(Properties.CreateProperties(new IPEndPoint(IPAddress.Any, 0), new ChannelPipe().SetCreateChannelAction(channel =>
                { })));
            netronics.Start();
            netronics.AddChannel(SocketChannel.CreateChannel(null));
            netronics.Stop();
        }
    }
}
