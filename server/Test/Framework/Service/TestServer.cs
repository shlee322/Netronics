using System.Threading;
using NUnit.Framework;
using Netronics.Template.Service.Service;

//using log4net;

namespace Framework.Service
{
    [TestFixture]
    public class TestServer
    {
        //private static readonly ILog Logger = LogManager.GetLogger(typeof(TestServer));  
        public static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        [Test]
        public void Test1()
        {
            var loginService = new LocalService("");
            loginService.AddRole<ILoginServer>(() => new LoginServer());
            var authService = new LocalService("");
            authService.AddRole<IAuthServer>(() => new AuthServer());


            var entity = loginService.NewEntity();
            entity.Call<ILoginServer>(server => server.Login("test", "test"));
            //ExitEvent.WaitOne();
        }
    }
}
