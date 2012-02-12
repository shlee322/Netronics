using Netronics.HandShake;
using Netronics.PacketEncoder;

namespace Netronics.Protocol
{
    public interface IProtocol
    {
        IPacketEncryptor GetEncryptor();
        IPacketDecryptor GetDecryptor();
        IPacketEncoder GetEncoder();
        IPacketDecoder GetDecoder();
        IHandShake GetHandShake();
    }
}