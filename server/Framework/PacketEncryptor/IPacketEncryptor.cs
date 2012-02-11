namespace Netronics
{
	public interface IPacketEncryptor
	{
		PacketBuffer Encrypt(Channel.Channel channel, PacketBuffer buffer);
	}
}

