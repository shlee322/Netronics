using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Template.Http
{
    public class HttpContact
    {
        private readonly IChannel _channel;
        private readonly Request _request;
        private readonly Response _response;

        public bool IsAutoSendResponse { get; set; }

        public HttpContact(IChannel channel, Request request, Response response = null)
        {
            _channel = channel;
            _request = request;
            _response = response;
            IsAutoSendResponse = true;
        }

        

        public IChannel GetChannel()
        {
            return _channel;
        }

        public Request GetRequest()
        {
            return _request;
        }

        public Response GetResponse()
        {
            return _response;
        }
    }
}
