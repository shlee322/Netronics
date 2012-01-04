using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Netronics
{
    /// <summary>
    /// 원격지의 서비스
    /// </summary>
    public class RemoteService : Service
    {
        protected byte[] socketBuffer = new byte[512];
        protected PacketBuffer packetBuffer = new PacketBuffer();
        protected Socket oSocket;
        protected string ServiceName;

        protected PacketEncoder packetEncoder;
        protected PacketDecoder packetDecoder;

        protected Transaction transaction = new Transaction();

        protected bool run = false;

        public RemoteService(Socket socket, PacketEncoder encoder, PacketDecoder decoder)
        {
            this.oSocket = socket;
            this.packetEncoder = encoder;
            this.packetDecoder = decoder;

            this.getSocket().BeginDisconnect(false, new AsyncCallback(this.disconnectCallback), null);
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

        protected void disconnectCallback(IAsyncResult ar)
        {
            this.run = false;
            this.getSocket().EndDisconnect(ar);
            //할당된 작업 해제 등등
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
                return;
            }

            this.getPacketBuffer().write(this.getSocketBuffer(), 0, len);

            LinkedList<dynamic> messageList = this.getPacketMessageList();

            Task task = new Task(() =>
                {
                    Parallel.ForEach(messageList, message =>
                    {
                        if (PacketProcessor.getPacketType(this, message) == "q")
                        {
                            Parallel.Invoke(() =>
                                {
                                    PacketProcessor.processingPacket(this, message);
                                });
                        }
                        else
                        {
                            Job job = this.transaction.getTransaction((string)message.t);
                            if (job != null)
                            {
                                Parallel.Invoke(() =>
                                    {
                                        job.returnResult(this, message.f != true ? true : false);
                                    });
                            }
                        }
                    });
                });

            this.getSocket().BeginReceive(this.getSocketBuffer(), 0, 512, SocketFlags.None, this.readCallback, null);

            task.Start();
        }

        private LinkedList<dynamic> getPacketMessageList()
        {
            LinkedList<dynamic> packetList = new LinkedList<dynamic>();

            dynamic packet;
            while ((packet = this.getPacketDecoder().decode(this.getPacketBuffer())) != null)
                packetList.AddLast(packet);

            return packetList;
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

        public string getServiceName()
        {
            return this.ServiceName;
        }

        public double getLoad()
        {
            return 0;
        }

        public bool getRunning()
        {
            return this.run;
        }

        public bool isGroup(string group)
        {
            return false;
        }

        public string[] getGroupArray()
        {
            return new string[] { };
        }

        public void init()
        {
        }

        public void start()
        {
        }

        public void stop()
        {
        }

        public bool sendMessage(dynamic data)
        {
            PacketBuffer buffer = this.getPacketEncoder().encode(data);
            bool r = this.sendPacket(buffer);
            buffer.Dispose();
            return r;
        }

        public bool sendPacket(PacketBuffer buffer)
        {
            byte[] sendBuffer = buffer.getBytes();
            try
            {
                this.getSocket().BeginSendTo(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, this.getSocket().RemoteEndPoint, new AsyncCallback(this.sendPacketCallback), null);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        private void sendPacketCallback(IAsyncResult ar)
        {
            this.getSocket().EndSendTo(ar);
        }

        public void processingJob(Service Service, Job job)
        {
            this.sendMessage(PacketProcessor.createQueryPacket(job.receiveResult ? this.transaction.createTransaction(job) : null, job));
        }
    }
}
