using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Netronics
{
    class RemoteSerivce : Serivce
    {

        protected byte[] socketBuffer = new byte[512];
        protected PacketBuffer packetBuffer = new PacketBuffer();
        protected Socket oSocket;
        protected string serivceName;

        public RemoteSerivce(Socket socket)
        {
            this.oSocket = socket;
            this.getSocket().BeginReceive(this.socketBuffer, 0, 512, SocketFlags.None, this.readCallback, null);
        }

        protected Socket getSocket()
        {
            return this.oSocket;
        }

        protected void readCallback(IAsyncResult ar)
        {
            int len = this.getSocket().EndReceive(ar);
            packetBuffer.write(this.socketBuffer, 0, len);
            object data = Netronics.getPacketDecoder().decode(packetBuffer);
            this.getSocket().BeginReceive(this.socketBuffer, 0, 512, SocketFlags.None, this.readCallback, null);
            if (data == null)
                return;


            //기본적인 서비스 패킷 처리하고
            //자기서비스의 processingJob로 넘기자!
            //Netronics.serivce.processingJob();
        }

        public string getSerivceName()
        {
            return this.serivceName;
        }

        public void start()
        {
        }

        public void stop()
        {
        }

        public void processingJob(Job job)
        {
            //패킷전송을 만들자.
        }
    }
}
