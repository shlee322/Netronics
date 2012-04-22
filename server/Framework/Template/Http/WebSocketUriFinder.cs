using System;
using System.Text.RegularExpressions;
using Netronics.Channel;

namespace Netronics.Template.Http
{
    class WebSocketUriFinder
    {
        private readonly Regex _rx;
        private readonly Func<string[], IChannelHandler> _handler;

        public WebSocketUriFinder(string uri, Func<string[], IChannelHandler> handler)
        {
            _rx = new Regex(uri);
            _handler = handler;
        }

        public bool IsMatch(string uri)
        {
            return _rx.IsMatch(uri);
        }

        public IChannelHandler GetHandler(string uri)
        {
            return _handler(_rx.Split(uri));
        }
    }
}
