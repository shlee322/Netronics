using System;
using Netronics;

namespace ProxyService
{
	public interface Handshake
	{
		IPacketEncoder GetPacketEncoder();
		IPacketDecoder GetPacketDecoder();
		
		bool ProcessingHandshake(Client client, dynamic message);
	}
}

