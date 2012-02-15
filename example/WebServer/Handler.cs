using Netronics.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace WebServer
{
    class Handler : IChannelHandler
    {
        public void Connected(IChannel channel)
        {
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            Request request = message;

            string useragent = request.GetHeader("User-Agent");

            Response response = new Response();
            if (useragent.IndexOf("MSIE", System.StringComparison.Ordinal) == -1)
                response.SetContent("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html\"; charset=\"UTF-8\" /></head><body>Test</body></html>");
            else
                response.SetContent("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html\"; charset=\"UTF-8\" /></head><body>IE Test</body></html>");

            channel.SendMessage(response);
        }
    }
}
