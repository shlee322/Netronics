using Netronics.Channel;
using Netronics.Protocol.PacketEncoder;

namespace Netronics.Protocol.HandShake
{
    interface IStep : IPacketEncoder, IPacketDecoder
    {
        void Start(HandShakeHandler handShakeHandler, IChannel channel);
        void End(HandShakeHandler handShakeHandler, IChannel channel);
        void MessageReceive(HandShakeHandler handShakeHandler, IChannel channel, dynamic message);
    }
}
