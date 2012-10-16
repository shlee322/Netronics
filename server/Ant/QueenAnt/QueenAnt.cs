using System.Net;
using Netronics.Channel;
using Netronics.Protocol.PacketEncoder.Bson;

namespace Netronics.Ant.QueenAnt
{
    class QueenAnt : IChannelHandler
    {
        private Netronics _netronics;

        public QueenAnt(QueenLoader loader)
        {
            _netronics = new Netronics(
                Properties.CreateProperties(
                new IPEndPoint(IPAddress.Any, loader.GetPort()),
                new ChannelPipe().SetCreateChannelAction(channel=>
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
        }
    }
}
