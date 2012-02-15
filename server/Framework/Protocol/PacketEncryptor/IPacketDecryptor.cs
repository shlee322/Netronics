using Netronics.Channel;

namespace Netronics.Protocol.PacketEncryptor
{
    public interface IPacketDecryptor
    {
        PacketBuffer Decrypt(IChannel channel, PacketBuffer buffer);
    }
}