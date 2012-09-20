using System;
using System.Collections.Generic;
using System.Linq;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Netronics.Template.Http.Handler
{
    public class HttpHandler : IChannelHandler
    {
        private readonly LinkedList<IUriHandler> _uriHandlers = new LinkedList<IUriHandler>();
        private readonly LinkedList<UriFinder> _wsHandlers = new LinkedList<UriFinder>();
        private readonly LinkedList<UriFinder> _jsonHandlers = new LinkedList<UriFinder>();
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        public void AddStatic(string uri, string path, string host = "")
        {
            _uriHandlers.AddLast(new StaticUriHandler(uri, path));
        }

        public void AddDynamic(string uri, Action<HttpContact> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(contact)));
        }

        public void AddDynamic(string uri, Action<HttpContact, string[]> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, action));
        }

        public void AddDynamic(string uri, Action<HttpContact, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
                args.Length < 1 ? null : args[0])));
        }

        public void AddDynamic(string uri, Action<HttpContact, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1])));
        }

        public void AddDynamic(string uri, Action<HttpContact, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2])));
        }

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3])));
        }

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4])));
        }

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5])));
        }

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6])));
        }

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
                args.Length < 1 ? null : args[0],
                args.Length < 2 ? null : args[1],
                args.Length < 3 ? null : args[2],
                args.Length < 4 ? null : args[3],
                args.Length < 5 ? null : args[4],
                args.Length < 6 ? null : args[5],
                args.Length < 7 ? null : args[6],
                args.Length < 8 ? null : args[7])));
        }

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
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

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
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

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
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

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
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

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
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

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
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

        public void AddDynamic(string uri, Action<HttpContact, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string> action)
        {
            _uriHandlers.AddLast(new DynamicUriHandler(uri, (contact, args) => action(
                contact,
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
                args.Length < 14 ? null : args[13],
                args.Length < 15 ? null : args[14])));
        }

        public void AddWebSocket(string uri, Func<string[], IChannelHandler> handler)
        {
            _wsHandlers.AddLast(new UriFinder(uri, handler));
        }

        public void AddJSON(string uri, Func<string[], JObject> handler)
        {
            _jsonHandlers.AddLast(new UriFinder(uri, handler));
        }

        public void AddSocketIO(SocketIO.SocketIO socketIO, string Namespace = "socket.io")
        {
            AddDynamic("^/" + Namespace + "/1/$", socketIO.Handshake);
            AddWebSocket("^/" + Namespace + "/1/websocket/(.*)$", socketIO.GetWebSocket);
            AddWebSocket("^/" + Namespace + "/1/flashsocket/(.*)$", socketIO.GetFlashSocket);
            AddDynamic("^/" + Namespace + "/1/xhr-polling/(.*)$", socketIO.XhrPolling);
            AddDynamic("^/" + Namespace + "/1/jsonp-polling/(.*)$", socketIO.JsonpPolling);
        }

        public void Connected(IReceiveContext context)
        {
        }

        public void Disconnected(IReceiveContext context)
        {
        }

        public void MessageReceive(IReceiveContext context)
        {
            var request = context.GetMessage() as Request;
            if(request == null)
                return;

            Processing(context.GetChannel(), request);

            if (request.GetHeader("Connection") == null || request.GetHeader("Connection") == "close")
                context.GetChannel().Disconnect();
        }

        private void Processing(IChannel channel, Request request)
        {
            //웹소켓 요청인가?
            if (request.GetHeader("connection") == "Upgrade")
            {
                if (request.GetHeader("upgrade") == "websocket")
                    UpgradeWebSocket(channel, request);
                return;
            }

            //JSON
            if (request.GetHeader("Accept") != null && request.GetHeader("Accept").IndexOf("application/json") >= 0)
            {
                UriFinder finder = GetJSONUriFinder(request);
                if (finder != null)
                {
                    var response = new Response();
                    response.SetContent(finder.GetHandler(request.GetPath()).ToString());
                    response.ContentType = "application/json";
                    response.Protocol = request.GetProtocol();
                    channel.SendMessage(response);
                    return;
                }
            }

            //여기서 여러가지 예외 처리를!
            IUriHandler handler = GetUriHandler(channel, request);
            if (handler != null)
            {
                handler.Handle(channel, request);
            }
            else
            {
                var response = new Response();
                response.Status = 401;
                response.Protocol = request.GetProtocol();
                channel.SendMessage(response);
            }
        }

        private void UpgradeWebSocket(IChannel channel, Request request)
        {
            var response = new Response { Status = 101, Protocol="1.1"};

            var finder = GetWebSocketUriFinder(request);
            if (finder == null)
                return;

            channel.SetConfig("handler", finder.GetHandler(request.GetPath()));
            //((IChannelHandler)channel.GetConfig("handler")).GetHandler();
            response.GetHeader().AppendLine("Upgrade: websocket")
                .AppendLine("Connection: Upgrade")
                .AppendLine("Sec-WebSocket-Accept: " + GetWebSocketAcceptCode(request.GetHeader("Sec-WebSocket-Key")));
            channel.SendMessage(response);
            channel.SetConfig("encoder", Protocol.PacketEncoder.WebSocket.WebSocketEncoder.Encoder);
            channel.SetConfig("decoder", Protocol.PacketEncoder.WebSocket.WebSocketDecoder.Decoder);
            //protocol.SetProtocol(WebSocketProtocol.Protocol);
            ((IChannelHandler)channel.GetConfig("handler")).Connected(null/**/);
            //handler.GetHandler().Connected(channel);
        }

        private string GetWebSocketAcceptCode(string code)
        {
            return Convert.ToBase64String(
                    new System.Security.Cryptography.SHA1Managed().ComputeHash(
                        System.Text.Encoding.UTF8.GetBytes(code + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11")));
        }

        private UriFinder GetWebSocketUriFinder(Request request)
        {
            return _wsHandlers.FirstOrDefault(handler => handler.IsMatch(request.GetPath()));
        }

        private UriFinder GetJSONUriFinder(Request request)
        {
            return _jsonHandlers.FirstOrDefault(handler => handler.IsMatch(request.GetPath()));
        }

        private IUriHandler GetUriHandler(IChannel channel, Request request)
        {
            return _uriHandlers.FirstOrDefault(handler => handler.IsMatch(request));
        }
    }
}
