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

        public RemoteSerivce(Socket socket)
        {
            this.oSocket = socket;
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
