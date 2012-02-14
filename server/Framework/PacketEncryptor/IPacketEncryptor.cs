using Netronics.Channel;

namespace Netronics
{
    public interface IPacketEncryptor
    {
        PacketBuffer Encrypt(IChannel channel, PacketBuffer buffer);
    }
}