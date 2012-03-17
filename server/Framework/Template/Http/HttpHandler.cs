using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddDynamic(string uri, Action<Request, Response> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(request, response)));
        }

        public void AddDynamic(string uri, Action<Request, Response, string[]> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, action));
        }

        public void AddDynamic(string uri, Action<Request, Response, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0]
                )));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request, 
                response, 
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6],
                args.Length < 8 ? null : args[7])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6],
                args.Length < 8 ? null : args[7],
                args.Length < 9 ? null : args[8])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6],
                args.Length < 8 ? null : args[7],
                args.Length < 9 ? null : args[8],
                args.Length < 10 ? null : args[9])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6],
                args.Length < 8 ? null : args[7],
                args.Length < 9 ? null : args[8],
                args.Length < 10 ? null : args[9],
                args.Length < 11 ? null : args[10])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6],
                args.Length < 8 ? null : args[7],
                args.Length < 9 ? null : args[8],
                args.Length < 10 ? null : args[9],
                args.Length < 11 ? null : args[10],
                args.Length < 12 ? null : args[11])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6],
                args.Length < 8 ? null : args[7],
                args.Length < 9 ? null : args[8],
                args.Length < 10 ? null : args[9],
                args.Length < 11 ? null : args[10],
                args.Length < 12 ? null : args[11],
                args.Length < 13 ? null : args[12])));
        }

        public void AddDynamic(string uri, Action<Request, Response, string, string, string, string, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (request, response, args) => action(
                request,
                response,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6],
                args.Length < 8 ? null : args[7],
                args.Length < 9 ? null : args[8],
                args.Length < 10 ? null : args[9],
                args.Length < 11 ? null : args[10],
                args.Length < 12 ? null : args[11],
                args.Length < 13 ? null : args[12],
                args.Length < 14 ? null : args[13])));
        }

        public void Connected(IChannel channel)
        {
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            IUriHandler handler = GetUriHandler(channel, message);
            if (handler != null)
            {
                handler.Handle(channel, message);
            }else
            {
                Response response = new Response();
                response.SetContent("404 Error!");
                channel.SendMessage(response);
            }
            channel.Disconnect();
        }

        private IUriHandler GetUriHandler(IChannel channel, Request request)
        {
            return _uriHandlers.FirstOrDefault(handler => handler.IsMatch(request));
        }
    }
}
