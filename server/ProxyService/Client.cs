using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Netronics;

namespace ProxyService
{
    public class Client
    {
        private int id;
        private Socket socket;
		private Handshake handshake;
		
		protected byte[] socketBuffer = new byte[512];
        protected PacketBuffer packetBuffer = new PacketBuffer();

        public Client(int id, Socket socket, Handshake handshake)
        {
            this.id = id;
            this.socket = socket;
			this.handshake = handshake;

            this.GetSocket().BeginReceive(socketBuffer, 0, 512, SocketFlags.None, readCallback, null);
        }
		
		private Socket GetSocket()
		{
			return this.socket;
		}
		
		private void disconnect()
		{
		}


        private PacketBuffer GetPacketBuffer()
        {
            return this.packetBuffer;
        }
		
		private void ProcessingHandshake()
		{
			IPacketDecoder packetDecoder = handshake.GetPacketDecoder();
            dynamic packet;
			while ((packet = packetDecoder.Decode(GetPacketBuffer())) != null)
			{
				if(this.handshake.ProcessingHandshake(this, packet))
				{
					this.handshake = null;
					break;
				}
			}
		}
		
		private void processingPacket()
		{
		}
		
        protected void readCallback(IAsyncResult ar)
        {
            int len;
            try
            {
                len = this.GetSocket().EndReceive(ar);
            }
            catch (SocketException)
            {
                this.disconnect();
                return;
            }

            this.GetPacketBuffer().Write(this.socketBuffer, 0, len);
			
			if(this.handshake != null)
				this.ProcessingHandshake();
			
			if(this.handshake == null)
				this.processingPacket();
			
            this.GetSocket().BeginReceive(this.socketBuffer, 0, 512, SocketFlags.None, this.readCallback, null);
        }
    }
}
