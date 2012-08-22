using System;
using System.Text.RegularExpressions;

namespace Netronics.Template.Http
{
    class UriFinder
    {
        private readonly Regex _rx;
        private readonly Func<string[], object> _handler;

        public UriFinder(string uri, Func<string[], object> handler)
        {
            _rx = new Regex(uri);
            _handler = handler;
        }

        public bool IsMatch(string uri)
        {
            return _rx.IsMatch(uri);
        }

        public object GetHandler(string uri)
        {
            return _handler(_rx.Split(uri));
        }
    }
}
