namespace Netronics
{
	public interface IPacketDecryptor
	{
		PacketBuffer Decrypt(PacketBuffer buffer);
	}
}

