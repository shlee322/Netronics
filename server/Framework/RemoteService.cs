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

        public RemoteService(Socket socket, PacketEncoder encoder, PacketDecoder decoder)
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
            DateTime time = DateTime.Now;
            this.getPacketBuffer().write(this.getSocketBuffer(), 0, len);

            Parallel.ForEach(this.getPacketMessageList(), message =>
            {
                if (PacketProcessor.getPacketType(this, message) == "q")
                {
                    new Task(new Action(delegate()
                        {
                            PacketProcessor.processingPacket(this, message);
                        })).Start();
                }
                else
                {
                    Job job = this.transaction.getTransaction((string)message.t);
                    if (job != null)
                    {
                        new Task(new Action(delegate()
                            {
                                job.returnResult(this, message.f != true ? true : false);
                            })).Start();
                    }
                }
            });
            System.Console.WriteLine(DateTime.Now.Ticks - time.Ticks);

            this.getSocket().BeginReceive(this.getSocketBuffer(), 0, 512, SocketFlags.None, this.readCallback, null);


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
            return this.sendPacket(this.getPacketEncoder().encode(data));
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
