using Netronics.Protocol.HandShake;
using Netronics.Protocol.PacketEncoder;

namespace Netronics.Protocol
{
    public interface IProtocol
    {
        IPacketEncoder GetEncoder();
        IPacketDecoder GetDecoder();
    }
}