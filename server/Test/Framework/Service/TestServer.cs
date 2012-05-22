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
            var service = new LocalService("");
            service.AddRole<ILoginServer>(() => new LoginServer());
            var entity = service.NewEntity();
            entity.Call<ILoginServer>(server => server.Login("test", "test"));
            //ExitEvent.WaitOne();
        }
    }
}
