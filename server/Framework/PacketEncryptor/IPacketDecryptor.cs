using Netronics.Channel;

namespace Netronics
{
    public interface IPacketDecryptor
    {
        PacketBuffer Decrypt(IChannel channel, PacketBuffer buffer);
    }
}