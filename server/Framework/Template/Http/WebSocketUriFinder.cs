using System;
using Netronics.Channel;

namespace Netronics.Template.Http
{
    class WebSocketUriFinder
    {
        private IChannelHandler _handler;

        public WebSocketUriFinder(string uri, Func<string[], IChannelHandler> handler)
        {
        }

        public bool IsMatch(string uri)
        {
            return false;
        }
    }
}
