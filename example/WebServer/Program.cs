﻿using System.Threading;
using Netronics.Template.Http;
using Netronics.Template.Http.Handler;
using Newtonsoft.Json.Linq;

namespace WebServer
{
    class Program
    {
        public static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var handler = new HttpHandler();
            handler.AddStatic("^/$", "./www/index.html");
            handler.AddStatic("^/file/(.*)$", "./www/test/file/{1}");

            handler.AddDynamic("^/test.web$", TestModule.TestController.TestMain);
            handler.AddWebSocket("^/echo$", strings => new WebSocketHandler());
            handler.AddJSON("^/test.json$", strings =>
                                                {
                                                    dynamic json = new JObject();
                                                    json.test = "abcd";
                                                    json.test2 = 123;
                                                    json.a = new JArray(strings);
                                                    return json;
                                                });

            handler.AddSocketIO(new SocketIO());

            var netronics = new Netronics.Netronics(new HttpProperties(() => handler));
            netronics.Start();
            ExitEvent.WaitOne();
        }
    }
}
