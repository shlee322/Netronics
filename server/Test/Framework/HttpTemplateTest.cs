﻿using System;
using System.Net;
using NUnit.Framework;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;
using Netronics.Template.HTTP;
using Netronics.Template.Http;

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
            handler.AddStatic("/file/(*)","./www/file/{0}");
            handler.AddDynamic("/test.web", requestData =>
                                                {
                                                    return new Response();
                                                });

            var netronics = new Netronics.Netronics(new HttpProperties(() => handler, 8888));
            netronics.Start();
            var request = WebRequest.Create("http://127.0.0.1:8888/a?b=c");
            request.GetResponse();
            netronics.Stop();
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
