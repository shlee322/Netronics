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
        private ISocketIO _socketIO;

        private readonly Dictionary<string, Action<dynamic>> _event = new Dictionary<string, Action<dynamic>>();

        public Client(HttpContact contact, ISocketIO socketIO)
        {
            _socketIO = socketIO;
            _channel = contact.GetChannel();
            _endPoint = contact.GetRequest().GetPath();
            _endPoint = _endPoint.Substring(0, _endPoint.Length - 3);
        }

        public IChannel GetChannel()
        {
            return _channel;
        }

        public override string ToString()
        {
            return _channel.ToString();
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
            _socketIO.Emit("connection", this);

            Send("1", "", _endPoint + "?server=netronics");
            if(_heartbeatTimer == null)
                _heartbeatTimer = new Timer(state => Send("2", ""), null, 0, 10000);
        }

        public void Disconnected(IChannel channel)
        {
            if(channel != _channel)
                return;

            if (_heartbeatTimer != null)
                _heartbeatTimer.Dispose();
            _heartbeatTimer = null;

            Send("0", "");

            _socketIO.Emit("disconnect", this);
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            if(message is ConnectionClose)
            {
                channel.Disconnect();
                return;
            }
            else if(message is byte[])
            {
                string msg = System.Text.Encoding.UTF8.GetString(message);

                if (msg.StartsWith("0"))
                {
                    channel.Disconnect();
                }
                else if (msg.StartsWith("3:::")) //Message
                {
                    if (msg.Length < 5)
                        return;
                    if (_event.ContainsKey("message"))
                        _event["message"](msg.Substring(4));
                }
                else if (msg.StartsWith("4:::")) //JSON Message
                {
                    if (msg.Length < 5)
                        return;
                    if (_event.ContainsKey("message"))
                        _event["message"](JsonConvert.DeserializeObject(msg.Substring(4)));

                }
                else if (msg.StartsWith("5:::")) //Event
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

        public void On(string id, Action<dynamic> action)
        {
            _event.Add(id, action);
        }

        public void Emit(string id, dynamic o)
        {
            dynamic data = new JObject();
            data.name = id;
            data.args = o;
            Send("5", "", JsonConvert.SerializeObject(data));
        }
    }
}
