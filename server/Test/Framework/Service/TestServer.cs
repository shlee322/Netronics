using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using NUnit.Framework;
using Netronics;
using Netronics.Channel.Channel;
using Netronics.Template.Service.Protocol;
using Netronics.Template.Service.Service;

namespace Framework.Service
{
    [TestFixture]
    public class TestServer
    {
        public static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        [Test]
        public void Test1()
        {
            var service = new LocalService(new byte[]{0,0,0,1});
            service.AddProcessor(typeof(TestMessage), TestProcessor.aaaa);

            Properties properties = new Netronics.Template.Service.ServiceProperties(service);
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 10050));
            var netronics = new Netronics.Netronics(properties);
            netronics.Start();

            var service2 = new LocalService(new byte[] { 0, 0, 0, 2 });
            service2.AddProcessor(typeof(TestMessage), TestProcessor.aaaa);

            Properties properties2 = new Netronics.Template.Service.ServiceProperties(service2);
            properties2.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 10051));
            var netronics2 = new Netronics.Netronics(properties2);
            netronics2.Start();
            IChannel test = properties2.GetChannelFactory().CreateChannel(netronics2, new TcpClient("127.0.0.1", 10050).Client);
            test.Connect();
            netronics2.AddChannel(test);

            test.SendMessage(new Request() { Sender = new byte[] { 0, 0, 0, 2 }, Receiver = new byte[] { 0, 0, 0, 1 }, Transaction = 0, Message = new TestMessage {name = "test", test = 123} });
            test.SendMessage(new Request() { Sender = new byte[] { 0, 0, 0, 2 }, Receiver = new byte[] { 0, 0, 0, 1 }, Transaction = 0, Message = new TestMessage { name = "test", test = 123 } });


            ExitEvent.WaitOne();
        }
    }
}
