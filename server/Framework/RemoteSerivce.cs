using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Netronics
{
    /// <summary>
    /// 원격지의 서비스
    /// </summary>
    public class RemoteSerivce : Serivce
    {
        protected byte[] socketBuffer = new byte[512];
        protected PacketBuffer packetBuffer = new PacketBuffer();
        protected Socket oSocket;
        protected string serivceName;

        protected PacketEncoder packetEncoder;
        protected PacketDecoder packetDecoder;

        public RemoteSerivce(Socket socket, PacketEncoder encoder, PacketDecoder decoder)
        {
            this.oSocket = socket;
            this.packetEncoder = encoder;
            this.packetDecoder = decoder;

            this.getSocket().BeginReceive(this.getSocketBuffer(), 0, 512, SocketFlags.None, this.readCallback, null);
        }

        public PacketEncoder getPacketEncoder()
        {
            return this.packetEncoder;
        }

        public PacketDecoder getPacketDecoder()
        {
            return this.packetDecoder;
        }

        protected void readCallback(IAsyncResult ar)
        {
            int len = this.getSocket().EndReceive(ar);
            this.getPacketBuffer().write(this.getSocketBuffer(), 0, len);

            LinkedList<dynamic> packetList = new LinkedList<dynamic>();

            dynamic packet;
            while ((packet = this.getPacketDecoder().decode(this.getPacketBuffer())) != null)
                packetList.AddLast(packet);
            packet = null;

            this.getSocket().BeginReceive(this.getSocketBuffer(), 0, 512, SocketFlags.None, this.readCallback, null);

            foreach(dynamic message in packetList)
                PacketProcessor.processingPacket(this, message);
        }

        public Socket getSocket()
        {
            return this.oSocket;
        }

        public byte[] getSocketBuffer()
        {
            return this.socketBuffer;
        }

        public PacketBuffer getPacketBuffer()
        {
            return this.packetBuffer;
        }

        public string getSerivceName()
        {
            return this.serivceName;
        }

        public float getLoad()
        {
            return 0;
        }

        public bool isGroup(string group)
        {
            return false;
        }

        public void start()
        {
        }

        public void stop()
        {
        }

        public void sendMessage(dynamic data)
        {
            this.sendPacket(this.getPacketEncoder().encode(data));
        }

        public void sendPacket(PacketBuffer buffer)
        {
            byte[] sendBuffer = buffer.getBufferStream().ToArray();
            this.getSocket().BeginSendTo(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, this.getSocket().RemoteEndPoint, new AsyncCallback(this.sendPacketCallback), null);
        }

        private void sendPacketCallback(IAsyncResult ar)
        {
        }

        public void processingJob(Serivce serivce, Job job)
        {
            this.sendMessage(PacketProcessor.createQueryPacket(job));
        }
    }
}
