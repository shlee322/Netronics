using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NUnit.Framework;
using Netronics;
using Netronics.Channel.Channel;
using Netronics.Template.Service.Service;
using Netronics.Template.Service.Task;

namespace Framework.Service
{
    [TestFixture]
    public class TestServer
    {
        public static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        [Test]
        public void Test1()
        {
            var service = new LocalService(1, "Test");
            service.AddProcessor(typeof(TestMessage), TestProcessor.aaaa);

            Properties properties = new Netronics.Template.Service.ServiceProperties(service);
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 10050));
            var netronics = new Netronics.Netronics(properties);
            netronics.Start();


            var service2 = new LocalService(2, "Proxy");
            service2.AddResultObject(typeof(TestResult));
            service2.AddProcessor(typeof(TestMessage), TestProcessor.aaaa);

            Properties properties2 = new Netronics.Template.Service.ServiceProperties(service2);
            properties2.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 10051));
            var netronics2 = new Netronics.Netronics(properties2);
            netronics2.Start();

            IChannel test = properties2.GetChannelFactory().CreateChannel(netronics2, new TcpClient("127.0.0.1", 10050).Client);
            test.Connect();
            netronics2.AddChannel(test);

            Thread.Sleep(500);

            //for (int i = 0; i < 50; i++)
            //    service2.GetServiceManager().GetService(1).ProcessingTask(Task.CreateTask(new TestMessage { Name = (i != 49 ? "test" : "" ), Test = DateTime.Now.Ticks }, Test1Result));

            ExitEvent.WaitOne();
        }

        public void Test1Result(Task task, object message)
        {
            var result = message as TestResult;
            if (result != null) Console.WriteLine(result.Msg);
            Console.WriteLine(DateTime.Now.Ticks);
            //if (result.Msg == "")
            //    ExitEvent.Set();
        }
    }
}
