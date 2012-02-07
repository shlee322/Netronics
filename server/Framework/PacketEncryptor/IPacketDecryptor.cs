namespace Netronics
{
	public interface IPacketDecryptor
	{
		PacketBuffer Decrypt(hannel.Channel channel, PacketBuffer buffer);
	}
}

