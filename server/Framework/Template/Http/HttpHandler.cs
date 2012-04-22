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
        private readonly LinkedList<WebSocketUriFinder> _wsHandlers = new LinkedList<WebSocketUriFinder>();

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

        public void AddWebSocket(string uri, Func<string[], IChannelHandler> handler)
        {
            _wsHandlers.AddLast(new WebSocketUriFinder(uri, handler));
        }

        public void Connected(IChannel channel)
        {
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            var request = message as Request;
            if(request == null)
                return;
            //웹소켓 요청인가?
            if(request.GetHeader("connection") == "Upgrade")
            {
                if(request.GetHeader("upgread") == "websocket")
                    UpgreadWebSocket(channel, request);
                return;
            }
            //여기서 여러가지 예외 처리를!
            IUriHandler handler = GetUriHandler(channel, message);
            if (handler != null)
            {
                handler.Handle(channel, message);
            }
            else
            {
                var response = new Response();
                response.Status = 401;
                channel.SendMessage(response);
            }

            if(((Request)message).GetHeader("Connection") == "close")
                channel.Disconnect();
        }

        private void UpgreadWebSocket(IChannel channel, Request request)
        {
            var response = new Response();
            response.Status = 101;

            var c = channel as IKeepProtocolChannel;
            if (c == null)
                return;

            //일단 해당 핸들러를 찾아서 핸들러를 변경해주고
            //그다음에 프로토콜을 웹소켓으로 변경시킨다.


            //c.SetProtocol();
        }

        private IUriHandler GetUriHandler(IChannel channel, Request request)
        {
            return _uriHandlers.FirstOrDefault(handler => handler.IsMatch(request));
        }
    }
}
