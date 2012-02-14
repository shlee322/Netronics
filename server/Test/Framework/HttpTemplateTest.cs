using System;
using System.Net;
using NUnit.Framework;
using Netronics.Channel;
using Netronics.PacketEncoder.Http;
using Netronics.Template.HTTP;

namespace Framework
{
    [TestFixture]
    class HttpTemplateTest
    {
        [Test]
        public void Test1()
        {
            var netronics = new Netronics.Netronics(new HttpProperties().SetHandler(() => new TestHandler()).SetIpEndPoint(new IPEndPoint(IPAddress.Any, 8888)));
            netronics.Start();
            WebRequest request = WebRequest.Create("http://127.0.0.1:8888");
            request.GetResponse();
            netronics.Stop();
            Netronics.Scheduler.SetThreadCount(0);
        }

        class TestHandler : IChannelHandler
        {
            public void Connected(IChannel channel)
            {
            }

            public void Disconnected(IChannel channel)
            {
            }

            public void MessageReceive(IChannel channel, dynamic message)
            {
                Request request = message;
                channel.SendMessage(new Response());
            }
        }
    }
}
