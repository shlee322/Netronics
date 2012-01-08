using System;
using Netronics;

namespace ProxyService
{
	public interface Handshake
	{
		Handshake getInstance(Client client);
		
		IPacketEncoder getPacketEncoder();
		IPacketDecoder getPacketDecoder();
		
		bool processingHandshake(Client client, dynamic message);
	}
}

