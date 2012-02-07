namespace Netronics
{
	public interface IPacketEncryptor
	{
		PacketBuffer Encrypt(PacketBuffer buffer);
	}
}

