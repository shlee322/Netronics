using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant.Transaction
{
    class RemoteTransaction : Transaction
    {
        private RemoteAnt _ant;
        private IChannel _channel;

        public RemoteTransaction(RemoteAnt ant)
        {
            _ant = ant;
        }

        public void AddChannel(IChannel channel)
        {
            _channel = channel;
        }

        protected override void SendTask2(int tid, int index, JToken o)
        {
            var packet = new JObject();
            packet.Add("type", "request");
            packet.Add("t_id", tid);
            packet.Add("m_type", index);
            packet.Add("args", o);
            _channel.SendMessage(packet);
        }


        public void SendResponseTask(int tId, JToken message)
        {
            var packet = new JObject();
            packet.Add("type", "response");
            packet.Add("t_id", tId);
            packet.Add("args", message);
            _channel.SendMessage(packet);
        }
    }
}
