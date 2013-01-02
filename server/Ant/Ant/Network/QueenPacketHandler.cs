using Netronics.Channel;
using Netronics.Protocol.PacketEncoder.Bson;
using Newtonsoft.Json.Linq;
using Netronics.Ant.Packet;

namespace Netronics.Ant.Ant.Network
{
    class QueenPacketHandler : IChannelHandler
    {
        private static QueenPacketHandler _instance;

        private Client _netronics;

        public static void Init()
        {
            new QueenPacketHandler();
        }

        public static QueenPacketHandler GetQueenPacketHandler()
        {
            return _instance;
        }

        private QueenPacketHandler()
        {
            _instance = this;

            _netronics = new Client(
                Properties.CreateProperties(
                Kernel.GetKernel().GetQueenIPEndPoint(),
                new ChannelPipe().SetCreateChannelAction(channel =>
                {
                    channel.SetConfig("encoder", BsonEncoder.Encoder);
                    channel.SetConfig("decoder", BsonDecoder.Decoder);
                    channel.SetConfig("handler", this);
                })));
            _netronics.Start();
        }

        public void Connected(IReceiveContext context)
        {
            Kernel.GetKernel().QueenConnected(context.GetChannel());
        }

        public void Disconnected(IReceiveContext context)
        {
        }

        public void MessageReceive(IReceiveContext context)
        {
            var packet = (JObject)context.GetMessage();
            System.Console.WriteLine(packet);
            string type = packet["type"].Value<string>();
            switch (type)
            {
                case "ant_name_list":
                    Kernel.GetKernel().InitAnts(((JArray)packet["args"]));
                    break;
                case "approve_join_ant":
                    var args = packet["args"].Value<JObject>();
                    Kernel.GetKernel().ApproveJoinAnt(args["id"].Value<int>(), args["network"].Value<JArray>().Values<JObject>());
                    break;
            }
        }
    }
}
