using System;

namespace ProxyService
{
	public interface Handshake
	{
		struct Handshake getInstance();
		
		PacketEncoder getPacketEncoder();
		PacketDecoder getPacketDecoder();
		
		bool processingHandshake(Client client);
	}
}

