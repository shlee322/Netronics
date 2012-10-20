using System;
using System.Text;
using Netronics.Channel.Channel;
using Netronics.Mobile.Auth;
using Newtonsoft.Json.Linq;

namespace Netronics.Mobile
{
    public class Client
    {
        private long _id = -1;
        private IChannel _channel;

        public Client(IChannel channel)
        {
            _channel = channel;
        }

        public int Ver { get; set; }

        public void Emit(string type, JToken o)
        {
            var obj = new JObject();
            obj.Add("type", "msg");
            obj.Add("name", type);
            obj.Add("arg", o);
            _channel.SendMessage(obj);
        }

        public long GetId()
        {
            return _id;
        }

        public void Auth(JObject authData)
        {
            string key;
            User user;

            if(authData == null)
            {
                key = AuthProcessor.GenerateKey();
                user = AuthProcessor.NewAuthUser(key);
            }
            else
            {
                key = authData.Value<string>("key");
                user = AuthProcessor.GetUser(authData.Value<int>("id"));

                if (user == null || user.Key != key) //비정상 계정으로 판단하고 새로운 계정을 발급
                {
                    user = AuthProcessor.NewAuthUser(key);
                }

                user.Key = AuthProcessor.GenerateKey();
                user.Save();
            }

            _id = user.Id;

            //유저에게 바뀐 정보를 전송
            var obj = new JObject();
            obj.Add("type", "auth_data");
            obj.Add("id", user.Id);
            obj.Add("key", user.Key);
            _channel.SendMessage(obj);
        }
    }
}
