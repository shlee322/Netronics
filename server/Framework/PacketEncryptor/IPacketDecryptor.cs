namespace Netronics
{
    public interface IPacketDecryptor
    {
        PacketBuffer Decrypt(Channel.Channel channel, PacketBuffer buffer);
    }
}