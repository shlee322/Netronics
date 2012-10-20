using Netronics.Channel;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Netronics.Mobile
{
    class Handler : IChannelHandler
    {
        private Mobile _mobile;
        private Client _client;

        public Handler(IChannel channel, Mobile mobile)
        {
            _mobile = mobile;
            _client = new Client(channel);
        }

        public void Connected(IReceiveContext context)
        {
            _mobile.Connected(_client);
        }

        public void Disconnected(IReceiveContext context)
        {
            _mobile.Disconnected(_client);
        }

        public void MessageReceive(IReceiveContext context)
        {
            var message = (JToken)context.GetMessage();
            switch (message.Value<string>("type"))
            {
                case "connect":
                    _client.Ver = message.Value<int>("ver");
                    
                    //인증 시스템을 사용한다
                    if(_mobile.UseAuth)
                    {
                        _client.Auth(message.Value<JObject>("auth"));
                    }
                    break;
                case "msg":
                    _mobile.Call(new Request() { Client = _client, Type = message.Value<string>("name"), Arg = message.Value<JToken>("arg") });
                    break;
                case "add_push":
                    _mobile.Push.AddPush(_client, message);
                    break;
                case "remove_push":
                    _mobile.Push.RemovePush(_client, message);
                    break;
            }
        }
    }
}
