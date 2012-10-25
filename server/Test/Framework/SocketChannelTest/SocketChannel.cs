using System.Threading;
using NUnit.Framework;
using Netronics.Protocol.PacketEncoder.Linefeed;

namespace Netronics.Test.SocketChannelTest
{
    [TestFixture]
    class SocketChannel
    {
        public static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        [Test]
        public void SocketChannelTest1()
        {
            System.Console.WriteLine("SocketChannel + ChannelPipe Test");

            var encoder = new LinefeedEncoder(System.Text.Encoding.Default);
            var server = new Server(encoder, encoder);
            var client = new Client(encoder, encoder);
            ExitEvent.WaitOne();
            client.Stop();
            server.Stop();
        }
    }
}
