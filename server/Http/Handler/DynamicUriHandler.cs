using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Netronics.Channel.Channel;
using Netronics.Http;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Http.Handler
{
    class DynamicUriHandler : IUriHandler
    {
        private readonly Regex _rx;
        private readonly Action<HttpContact, string[]> _action;

        public DynamicUriHandler(string uri, Action<HttpContact, string[]> action)
        {
            _rx = new Regex(uri);
            _action = action;
        }

        public string GetUri()
        {
            return _rx.ToString();
        }

        public bool IsMatch(Request request)
        {
            return _rx.IsMatch(request.GetPath());
        }

        public void Handle(IChannel channel, Request request)
        {
            var contact = new HttpContact(channel, request, new Response {Protocol = request.GetProtocol()});
            if(contact.GetRequest().GetProtocol() == "HTTP/1.1")
                contact.GetResponse().Protocol = "1.1";

            try
            {
                _action(contact, _rx.Split(request.GetPath()));
            }
            catch (Exception e)
            {
                contact.GetResponse().Status = 500;
                contact.GetResponse().SetContent(e.ToString());
            }

            if (contact.IsAutoSendResponse)
                channel.SendMessage(contact.GetResponse());
        }
    }
}
