using System;
using System.Collections.Generic;
using System.Threading;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Netronics.Template.Http.SocketIO
{
    class Client : ISocketIO, IChannelHandler
    {
        public string Key { get; set; }
        private IChannel _channel;
        private readonly string _endPoint;
        private Timer _heartbeatTimer;

        private Dictionary<string, Action<ISocketIO>> _serverEvent;
        private Dictionary<string, Action<JArray>> _event = new Dictionary<string, Action<JArray>>();

        public Client(HttpContact contact, Dictionary<string, Action<ISocketIO>> serverEvent)
        {
            _channel = contact.GetChannel();
            _endPoint = contact.GetRequest().GetPath();
            _endPoint = _endPoint.Substring(0, _endPoint.Length - 3);
            _serverEvent = serverEvent;
        }

        public IChannel GetChannel()
        {
            return _channel;
        }

        public void Connected(IChannel channel)
        {
            if (channel.ToString().Substring(0, channel.ToString().IndexOf(":", StringComparison.Ordinal))
                != _channel.ToString().Substring(0, _channel.ToString().IndexOf(":", StringComparison.Ordinal)))
            {
                channel.Disconnect();
                return;
            }

            _channel = channel;


            var keepHandlerChannel = _channel as IKeepHandlerChannel;
            if (keepHandlerChannel != null)
            {
                keepHandlerChannel.SetHandler(this);
            }

            if (_serverEvent.ContainsKey("connection"))
                _serverEvent["connection"](this);

            Send("1", "", _endPoint + "?server=netronics");
            if(_heartbeatTimer == null)
                _heartbeatTimer = new Timer(state => Send("2", ""), null, 0, 10000);
        }

        public void Disconnected(IChannel channel)
        {
            if(channel != _channel)
                return;

            _heartbeatTimer.Dispose();
            _heartbeatTimer = null;
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            if(message is ConnectionClose)
            {
                channel.Disconnect();
                return;
            }else if(message is byte[])
            {
                string msg = System.Text.Encoding.UTF8.GetString(message);

                if (msg.StartsWith("5:::"))
                {
                    if (msg.Length < 5)
                        return;
                    dynamic data = JsonConvert.DeserializeObject(msg.Substring(4));
                    if(_event.ContainsKey(data.name.ToString()))
                        _event[data.name.ToString()](data.args);
                }
            }

        }

        private void Send(string type, string id, string data = null)
        {
            _channel.SendMessage(string.Format("{0}:{1}:{2}", type, id, /*_endPoint*/"") + (data != null ? string.Format(":{0}", data) : ""));
        }

        public void On(string id, Action<JArray> action)
        {
            _event.Add(id, action);
        }

        public void Emit(string id, JArray o)
        {
            dynamic data = new JObject();
            data.name = id;
            data.args = o;
            Send("5", "", JsonConvert.SerializeObject(data));
        }
    }
}
