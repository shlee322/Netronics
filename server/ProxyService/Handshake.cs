using System;

namespace ProxyService
{
	public interface Handshake
	{
		struct Handshake getInstance(Client client);
		
		PacketEncoder getPacketEncoder();
		PacketDecoder getPacketDecoder();
		
		bool processingHandshake(Client client);
	}
}

