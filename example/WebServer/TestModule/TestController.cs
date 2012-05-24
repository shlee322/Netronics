using Netronics.Protocol.PacketEncoder.Http;
using Netronics.Template.Http;
using Netronics.Template.Http.Module;

namespace WebServer.TestModule
{
    [Module]
    class TestController
    {
        public static void TestMain(HttpContact contact)
        {
            contact.GetResponse().SetTemplate<Request>("./www/test.cshtml", contact.GetRequest());
        }
    }
}
