using System;
using System.Text;
using Netronics.Channel.Channel;
using Netronics.Mobile.Auth;
using Newtonsoft.Json.Linq;

namespace Netronics.Mobile
{
    public class Client
    {
        private long _index = -1;
        private IChannel _channel;

        public Client(IChannel channel)
        {
            _channel = channel;
        }

        public int Ver { get; set; }

        public void Emit(string type)
        {
        }

        public long GetIndex()
        {
            return _index;
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

                if(user == null)
                {
                    //계정 증발
                }

                if(user.Key != key)
                {
                    //인증키 오류
                    _channel.Disconnect();
                    return;
                }

                user.Key = AuthProcessor.GenerateKey();
                user.Save();
            }

            _index = user.Id;

            //유저에게 바뀐 정보를 전송
            var obj = new JObject();
            obj.Add("type", "auth_data");
            obj.Add("id", user.Id);
            obj.Add("key", user.Key);
            _channel.SendMessage(obj);
        }
    }
}
