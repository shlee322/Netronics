using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Netronics.DB;
using Netronics.DB.Where;
using Newtonsoft.Json.Linq;

namespace Netronics.Mobile.Push
{
    public class Push
    {
        private string _gcmId;
        private string _gcmKey;

        public Push(Mobile mobile)
        {
        }

        public void SetGCMKey(string id, string key)
        {
            _gcmId = id;
            _gcmKey = key;
        }

        public void SendMessage(Client client, JToken obj)
        {
            SendMessage(client.GetId(), obj);
        }

        public void SendMessage(long id, JToken obj)
        {
            var data = Model.ModelObjects<User>().Find(new Where().Match(new User() { UserId = id }));
            if (data.Length == 0)
                return;
            var user = (User)data[0];
            if(user.Type == "GCM")
            {
                WebRequest request;
                request = WebRequest.Create("https://android.googleapis.com/gcm/send");
                request.Method = "post";
                request.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
                request.Headers.Add(string.Format("Authorization: key={0}", _gcmKey));

                request.Headers.Add(string.Format("Sender: id={0}", _gcmId));

                string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + HttpUtility.UrlEncode(obj.ToString()) + "&data.time=" + DateTime.Now.ToString() + "&registration_id=" + user.Key + "";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;

                var dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                var tResponse = request.GetResponse();

                dataStream = tResponse.GetResponseStream();

                var tReader = new StreamReader(dataStream);

                var sResponseFromServer = tReader.ReadToEnd();
                tReader.Close();
                dataStream.Close();
                tResponse.Close();
            }

        }

        public void AddPush(Client client, JToken message)
        {
            var data = Model.ModelObjects<User>().Find(new Where().Match(new User() {UserId = client.GetId()}));
            User user;

            //한번도 푸시 키가 등록된적 없음
            if(data.Length == 0)
            {
                user = new User() {UserId = client.GetId()};
            }
            else
            {
                user = (User)data[0];
            }
            
            user.Type = message.Value<string>("name");
            user.Key = message.Value<string>("arg");
            user.Save();
        }

        public void RemovePush(Client client, JToken message)
        {
        }
    }
}
