using System;
using Netronics;

namespace ProxyService
{
	public interface Handshake
	{
		Handshake getInstance(Client client);
		
		PacketEncoder getPacketEncoder();
		PacketDecoder getPacketDecoder();
		
		bool processingHandshake(Client client, dynamic message);
	}
}

