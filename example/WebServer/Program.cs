using System;
using System.Globalization;
using System.Net;
using System.Threading;
using Netronics;
using Netronics.Protocol.PacketEncoder.Http;
using Netronics.Template.HTTP;
using Netronics.Template.Http;

namespace WebServer
{
    class Program
    {
        public static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var handler = new HttpHandler();
            handler.AddStatic("/$", "./www/index.html");
            handler.AddStatic("^/file/(.*)$", "./www/test/file/{1}");

            handler.AddDynamic("/test.web", TestModule.TestController.TestMain);

            var netronics = new Netronics.Netronics(new HttpProperties(() => handler, 7777));
            netronics.Start();
            ExitEvent.WaitOne();
        }
    }
}
