using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace ProxyService
{
    class Client
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
			this.handshake = handshake.getInstance(this);
			
			this.getSocket().BeginReceive(this.getSocketBuffer(), 0, 512, SocketFlags.None, this.readCallback, null);
        }
		
		private Socket getSocket()
		{
			return this.socket;
		}
		
		private void disconnect()
		{
		}
		
		private void processingHandshake()
		{
			PacketDecoder packetDecoder = this.handshake.getPacketDecoder();
			while ((packet = packetDecoder.decode(this.getPacketBuffer())) != null)
			{
				if(this.handshake.processingHandshake(packetBuffer))
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
                len = this.getSocket().EndReceive(ar);
            }
            catch (SocketException)
            {
                this.disconnect();
                return;
            }

            this.getPacketBuffer().write(this.getSocketBuffer(), 0, len);
			
			if(this.handshake != null)
				this.processingHandshake();
			
			if(this.handshake == null)
				this.processingPacket();
			
            this.getSocket().BeginReceive(this.getSocketBuffer(), 0, 512, SocketFlags.None, this.readCallback, null);
        }
    }
}
