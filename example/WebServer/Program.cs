using System.Threading;
using Netronics.Template.Http;
using Netronics.Template.Http.Handler;
using Netronics.Template.Http.SocketIO;
using Newtonsoft.Json;
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
            handler.AddStatic("^/script/(.*)$", "./www/script/{1}");
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
            var io = new SocketIO();
            io.On("connection", s =>
                                    {
                                        var socket = s as ISocketIO;
                                        socket.Emit("news", new JArray(JsonConvert.DeserializeObject("{ hello: 'world' }")));
                                        socket.On("my other event", array => System.Console.WriteLine(array[0].my));
                                    });
            handler.AddSocketIO(io);

            var netronics = new Netronics.Netronics(new HttpProperties(() => handler));
            netronics.Start();
            ExitEvent.WaitOne();
        }
    }
}
