using System.Net;
using System.Net.Sockets;
using Netronics.Channel;
using Netronics.Protocol.PacketEncoder.Bson;
using Netronics.Ant.Packet;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant.Network
{
    class AntPipeline : IChannelHandler
    {
        private static AntPipeline _instance;

        private Netronics _netronics;

        public static void Init()
        {
            new AntPipeline();
        }

        public static AntPipeline GetAntPipeline()
        {
            return _instance;
        }

        private AntPipeline()
        {
            _instance = this;

            _netronics = new Netronics(
                Properties.CreateProperties(
                new IPEndPoint(IPAddress.Any, 0),
                new ChannelPipe().SetCreateChannelAction(channel =>
                {
                    channel.SetConfig("encoder", BsonEncoder.Encoder);
                    channel.SetConfig("decoder", BsonDecoder.Decoder);
                    channel.SetConfig("handler", this);
                })));

            _netronics.Start();
        }

        public int GetPort()
        {
            return _netronics.GetEndIPPoint().Port;
        }

        public void AddPipeline(RemoteAnt ant, IPEndPoint endPoint)
        {
            _netronics.AddSocket(endPoint, (netronics, channel) =>
                {
                    ant.AddChannel(channel);
                    var packet = new JObject();
                    packet.Add("ant", Kernel.GetKernel().GetLocalAnt().GetAnts().GetId());
                    packet.Add("id", Kernel.GetKernel().GetLocalAnt().GetId());
                    channel.SendMessage("hello_ant", packet);
                });
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
            string type = packet["type"].Value<string>();
            switch (type)
            {
                case "hello_ant":
                    var args = packet["args"].Value<JObject>();
                    Kernel.GetKernel().HelloAnt(context.GetChannel(), args.Value<int>("ant"), args.Value<int>("id"));
                    break;
                case "request":
                    var ant = context.GetChannel().GetTag() as RemoteAnt;
                    ant.ReceiveTask(packet["t_id"].Value<int>(), packet["m_type"].Value<int>(), packet["args"]);
                    break;
                case "response":
                    ant = context.GetChannel().GetTag() as RemoteAnt;
                    ant.ResponseTask(packet["t_id"].Value<int>(), packet["args"]);
                    break;
                default:
                    System.Console.WriteLine(packet);
                    break;
            }
        }
    }
}
