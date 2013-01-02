using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Packet
{
    static class Packet
    {
        public static void SendMessage(this IChannel channel, string type, JToken args)
        {
            var packet = new JObject();
            packet.Add("type", type);
            packet.Add("args", args);
            channel.SendMessage(packet);
        }
    }
}
