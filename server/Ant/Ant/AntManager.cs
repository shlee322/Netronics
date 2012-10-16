using Netronics.Channel;
using Netronics.Protocol.PacketEncoder.Bson;

namespace Netronics.Ant.Ant
{
    class AntManager : IChannelHandler
    {
        private Client _queenAnt;

        public AntManager(AntLoader loader)
        {
            _queenAnt = new Client(
                Properties.CreateProperties(
                loader.GetQueenIPEndPoint(),
                new ChannelPipe().SetCreateChannelAction(channel =>
                {
                    channel.SetConfig("encoder", BsonEncoder.Encoder);
                    channel.SetConfig("decoder", BsonDecoder.Decoder);
                    channel.SetConfig("handler", this);
                })));
            _queenAnt.Start();
        }

        public void Connected(IReceiveContext context)
        {
        }

        public void Disconnected(IReceiveContext context)
        {
        }

        public void MessageReceive(IReceiveContext context)
        {
        }
    }
}
