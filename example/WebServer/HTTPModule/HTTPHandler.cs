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
            Request request = message;

            string useragent = request.GetHeader("User-Agent");

            Response response = new Response();
            if (useragent.IndexOf("MSIE", System.StringComparison.Ordinal) == -1)
                response.SetContent("Test");
            else
                response.SetContent("IE 뻐큐머겅");

            client.Send(response);
        }
    }
}
