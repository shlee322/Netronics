using System;
using System.Net;
using System.Reflection;
using NUnit.Framework;
using Netronics;
using Netronics.Template.Service.Service;

namespace Framework.Service
{
    [TestFixture]
    public class TestServer
    {
        [Test]
        public void Test1()
        {
            DateTime time = DateTime.Now;
            Type t = Assembly.GetAssembly(GetType()).GetType(typeof (TestMessage).FullName);
            DateTime time2 = DateTime.Now;
            var service = new LocalService();
            service.AddProcessor(typeof(TestMessage), TestProcessor.aaaa);

            Properties properties = new Netronics.Template.Service.ServiceProperties(service);
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 10050));
            var netronics = new Netronics.Netronics(properties);
            netronics.Start();

            int a = 1;
        }
    }
}
