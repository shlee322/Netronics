using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncryptor
{
    public interface IPacketDecryptor
    {
        PacketBuffer Decrypt(IChannel channel, PacketBuffer buffer);
    }
}