using System;
using Netronics;

namespace ProxyService
{
    public interface Receiver
	{
		IPacketEncoder GetPacketEncoder();
		IPacketDecoder GetPacketDecoder();
		
		void Processing(Client client, dynamic message);
	}
}

