using Netronics.Protocol.HandShake;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncryptor;

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