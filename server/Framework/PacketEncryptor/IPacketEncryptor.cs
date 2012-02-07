namespace Netronics
{
	public interface IPacketEncryptor
	{
		PacketBuffer Encrypt(hannel.Channel channel, PacketBuffer buffer);
	}
}

