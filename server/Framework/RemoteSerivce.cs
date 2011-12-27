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
            dynamic packet = this.getPacketDecoder().decode(this.getPacketBuffer());
            this.getSocket().BeginReceive(this.getSocketBuffer(), 0, 512, SocketFlags.None, this.readCallback, null);

            if (packet == null || packet.type.GetType() != typeof(string))
                return;

            PacketProcessor.processingPacket(this, packet);
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
        }

        public void processingJob(Serivce serivce, Job job)
        {
            //패킷전송을 만들자.
        }
    }
}
