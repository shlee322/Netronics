using Netronics;
using ProxyService;

namespace HTTPModule
{
    class HTTPHandler : ProxyService.Receiver
    {
        private static HTTPDecoder decoder = new HTTPDecoder();
        private static HTTPEncoder encoder = new HTTPEncoder();

        public IPacketEncoder GetPacketEncoder()
        {
            return encoder;
        }

        public IPacketDecoder GetPacketDecoder()
        {
            return decoder;
        }

        public void Processing(Client client, dynamic message)
        {
            Response response = new Response();
            response.SetContent("Test");
            client.Send(response);
        }
    }
}
