using System;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Template.Http
{
    public class SocketIO : IChannelHandler
    {/*
        public void On(string type, Action<SocketIOChannel> action)
        {
        }*/

        public void Emit(string type, object message)
        {
        }


        public IChannelHandler GetWebSocket()
        {
            return this;
        }
        public IChannelHandler GetFlashSocket()
        {
            return null;
        }

        public void XhrPolling(Request request, Response response, string[] args)
        {
            if (args.Length < 3)
                return;

            var channel = GetPollingClient(args[2]);
            response.GetHeader().AppendLine("Access-Control-Allow-Origin: *");
            response.GetHeader().AppendLine("Access-Control-Allow-Credentials: true");
            //response.SetContent("0::/socket.io");
        }


        private IChannel GetPollingClient(string id)
        {
            return null;
        }



        public void Connected(IChannel channel)
        {
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
        }
    }
}
