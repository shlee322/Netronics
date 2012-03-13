using System;
using System.Collections.Generic;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Template.Http
{
    public class HttpHandler : IChannelHandler
    {
        private readonly LinkedList<IUriHandler> _uriHandlers = new LinkedList<IUriHandler>();

        public void AddStatic(string uri, string path, string host = "")
        {
            _uriHandlers.AddLast(new StaticUriHandler(uri, path));
        }

        public void AddDynamic(string uri, Func<Request, Response> func, string host = "")
        {
        }

        public void Connected(IChannel channel)
        {
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            Request request = message;
            foreach (var handler in _uriHandlers)
            {
                if(handler.IsMatch(request))
                {
                    handler.Handle(channel, request);
                    break;
                }
            }
        }
    }
}
