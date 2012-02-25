using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncryptor
{
    public interface IPacketEncryptor
    {
        PacketBuffer Encrypt(IChannel channel, PacketBuffer buffer);
    }
}