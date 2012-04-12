using Netronics.Protocol.PacketEncoder.Http;
using Netronics.Template.Http.Module;

namespace WebServer.TestModule
{
    [Module]
    class TestController
    {
        public static void TestMain(Request request, Response response)
        {
            response.SetTemplate<Request>("./www/test.cshtml", request);
        }
    }
}
