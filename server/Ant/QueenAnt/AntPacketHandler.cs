using System.Collections.Generic;
using System.Net;
using Netronics.Ant.Packet;
using Netronics.Channel;
using Netronics.Protocol.PacketEncoder.Bson;
using Netronics.Protocol.PacketEncoder.MsgPack;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.QueenAnt
{
    class AntPacketHandler : IChannelHandler
    {
        private static AntPacketHandler _instance;

        private Netronics _netronics;

        public static void Init()
        {
            new AntPacketHandler();
        }

        private AntPacketHandler()
        {
            _instance = this;

            _netronics = new Netronics(
                Properties.CreateProperties(
                new IPEndPoint(IPAddress.Any, QueenAnt.GetQueenAnt().GetPort()),
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
                case "get_ant_name_list":
                    context.GetChannel().SendMessage("ant_name_list", new JArray(QueenAnt.GetQueenAnt().GetAntNameArray()));
                    break;
                case "request_join_ant":
                    AntManager.GetAntManager().RequestJoinAnt(context.GetChannel(), packet["args"].Value<JObject>());
                    break;
                case "start_ant":
                    AntManager.GetAntManager().StartAnt(context.GetChannel().GetTag() as Ant);
                    break;
            }
        }
    }
}
