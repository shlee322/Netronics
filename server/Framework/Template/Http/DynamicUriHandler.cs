﻿using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Template.Http
{
    class DynamicUriHandler : IUriHandler
    {
        private readonly Regex _rx;
        private readonly Action<Request, Response, string[]> _action;

        public DynamicUriHandler(string uri, Action<Request, Response, string[]> action)
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
            var response = new Response();
            try
            {
                _action(request, response, _rx.Split(request.GetPath()));
            }
            catch (Exception e)
            {
                response.Status = 500;
                response.SetContent(e.ToString());
            }
            
            channel.SendMessage(response);
        }
    }
}
