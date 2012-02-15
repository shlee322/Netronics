using Netronics.Channel;

namespace Netronics.Protocol.PacketEncryptor
{
    public interface IPacketEncryptor
    {
        PacketBuffer Encrypt(IChannel channel, PacketBuffer buffer);
    }
}