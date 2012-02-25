using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;

namespace Netronics.Protocol.HandShake
{
    interface IStep : IPacketEncoder, IPacketDecoder
    {
        void Start(HandShake handShakeHandler, IChannel channel);
        void End(HandShake handShakeHandler, IChannel channel);
        void MessageReceive(HandShake handShakeHandler, IChannel channel, dynamic message);
    }
}
