using System;
using System.Globalization;
using System.Net;
using System.Threading;
using NUnit.Framework;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;
using Netronics.Template.Http;
using Netronics.Template.Http.Handler;

namespace Framework
{
    [TestFixture]
    class HttpTemplateTest
    {
        [Test]
        public void Test1()
        {
            var netronics = new Netronics.Netronics(new HttpProperties(()=>new TestHandler(), 8888));
            netronics.Start();
            var request = WebRequest.Create("http://127.0.0.1:8888/a?b=c");
            request.GetResponse();
            netronics.Stop();
        }

        [Test]
        public void Test2()
        {
            var handler = new HttpHandler();
            handler.AddStatic("/file/(*)","./www/file/{0}");/*
            handler.AddDynamic("/test.web", requestData =>
                                                {
                                                    return new Response();
                                                });*/

            var netronics = new Netronics.Netronics(new HttpProperties(() => handler, 8888));
            netronics.Start();
            var request = WebRequest.Create("http://127.0.0.1:8888/a?b=c");
            request.GetResponse();
            netronics.Stop();
        }

        public static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);
        [Test]
        public void Test3()
        {
            var handler = new HttpHandler();
            handler.AddStatic("/$", "./www/index.html");
            handler.AddStatic("^/file/(.*)$", "./www/test/file/{1}");

            handler.AddDynamic("/test.web", TestAction);
            var netronics = new Netronics.Netronics(new HttpProperties(() => handler, 7777));
            netronics.Start();
            ExitEvent.WaitOne();
        }

        private void TestAction(HttpContact contact)
        {
            contact.GetResponse().SetContent(DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        class TestHandler : IChannelHandler
        {
            public void Connected(IReceiveContext context)
            {
            }

            public void Disconnected(IReceiveContext context)
            {
            }

            public void MessageReceive(IReceiveContext context)
            {
                Request request = context.GetMessage() as Request;
                context.GetChannel().SendMessage(new Response());
            }
        }
    }
}
