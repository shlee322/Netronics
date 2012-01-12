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
        private Receiver receiver;
        private Receiver processor;
		
		protected byte[] socketBuffer = new byte[512];
        protected PacketBuffer packetBuffer = new PacketBuffer();

        public Client(int id, Socket socket, Receiver handshake, Receiver processor)
        {
            this.id = id;
            this.socket = socket;
            this.receiver = handshake;
            this.processor = processor;

            if (this.receiver == null)
                this.receiver = this.processor;

            this.GetSocket().BeginReceive(socketBuffer, 0, 512, SocketFlags.None, readCallback, null);
        }
		
		private Socket GetSocket()
		{
			return this.socket;
		}
		
		private void disconnect()
		{
		}

        public void EndHandshake()
        {
            this.receiver = this.processor;
        }

        public void Send(dynamic message)
        {
            PacketBuffer buffer = receiver.GetPacketEncoder().Encode(message);
            byte[] sendBuffer = buffer.GetBytes();
            try
            {
                GetSocket().BeginSendTo(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, GetSocket().RemoteEndPoint,
                                        SendPacketCallback, null);
            }
            catch (System.Exception)
            {
            }
            buffer.Dispose();
        }

        private void SendPacketCallback(IAsyncResult ar)
        {
        }

        private PacketBuffer GetPacketBuffer()
        {
            return this.packetBuffer;
        }
		
		private void Processing()
		{
            IPacketDecoder packetDecoder = receiver.GetPacketDecoder();
            dynamic packet;
            while ((packet = packetDecoder.Decode(GetPacketBuffer())) != null)
                this.receiver.Processing(this, packet);
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

            Processing();
			
            this.GetSocket().BeginReceive(this.socketBuffer, 0, 512, SocketFlags.None, this.readCallback, null);
        }
    }
}
