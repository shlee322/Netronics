using System;
using System.Collections.Generic;
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
        private static readonly List<ISocketIO> UserList = new List<ISocketIO>();
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
            };

            var handler = new HttpHandler();
            handler.AddStatic("^/$", "./www/index.html");
            handler.AddStatic("^(.*).css$", "./www/{1}.css");
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
                                        lock (UserList)
                                        {
                                            UserList.Add(socket);

                                            dynamic obj = new JObject();
                                            obj.name = "System";
                                            obj.msg = socket.ToString() + " 접속";
                                            foreach (var sock in UserList)
                                            {
                                                sock.Emit("chat", new JArray(new object[] { obj }));
                                            }
                                        }

                                        socket.On("chat", o =>
                                                              {
                                                                  dynamic obj = new JObject();
                                                                  obj.name = socket.ToString();
                                                                  obj.msg = o[0].msg;
                                                                  lock (UserList)
                                                                  {
                                                                      foreach (var sock in UserList)
                                                                      {
                                                                          sock.Emit("chat", new JArray(new object[] { obj }));
                                                                      }
                                                                  }
                                                              });
                                    });
            io.On("disconnect", o =>
                                    {
                                        var socket = o as ISocketIO;
                                        lock (UserList)
                                        {
                                            UserList.Remove(socket);

                                            dynamic obj = new JObject();
                                            obj.name = "System";
                                            obj.msg = socket.ToString() + " 접속 종료";
                                            foreach (var sock in UserList)
                                            {
                                                sock.Emit("chat", new JArray(new object[] { obj }));
                                            }
                                        }
                                    });
            handler.AddSocketIO(io);

            var netronics = new Netronics.Netronics(new HttpsProperties(() => handler));
            netronics.Start();
            ExitEvent.WaitOne();
        }
    }
}
