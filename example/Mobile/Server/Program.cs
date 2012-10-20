using System.Security.Cryptography.X509Certificates;
using Netronics.DB.DBMS;
using Netronics.Mobile;
using Newtonsoft.Json.Linq;

namespace MobileServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Sqlite.UsingSqlite("test.db");

            var mobile = new Mobile(7777, new X509Certificate2("server.pfx", "asdf"));
            mobile.UseAuth = true;
            mobile.Connected = client =>
                {
                };
            mobile.Disconnected = client =>
                {
                };
            mobile.On("hi", request =>
                {
                    request.Client.Emit("hi", new JValue("test222"));
                    mobile.Push.SendMessage(request.Client, "push_test", JToken.Parse("{test:123}"));
                });
            mobile.On("hi", request =>
                {
                    
                }, 5);
            mobile.On("hi2", request =>
            {

            });
            mobile.Push.SetGCMKey("568476072992", "AIzaSyAbX7hP5h29tgUczJDqhtntJavHTkvvodU");
            mobile.Run();
        }
    }
}
