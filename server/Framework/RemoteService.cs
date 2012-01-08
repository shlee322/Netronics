using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Netronics
{
    /// <summary>
    /// 원격지의 서비스
    /// </summary>
    public class RemoteService : Service
    {
        protected string ServiceName;
        protected Socket oSocket;
        protected PacketBuffer packetBuffer = new PacketBuffer();

        protected PacketDecoder packetDecoder;
        protected PacketEncoder packetEncoder;

        protected bool run;
        protected byte[] socketBuffer = new byte[512];
        protected Transaction transaction;

        public RemoteService(Socket socket, PacketEncoder encoder, PacketDecoder decoder)
        {
            oSocket = socket;
            packetEncoder = encoder;
            packetDecoder = decoder;
            transaction = new Transaction();

            getSocket().BeginReceive(getSocketBuffer(), 0, 512, SocketFlags.None, readCallback, null);
        }

        #region Service Members

        public string getServiceName()
        {
            return ServiceName;
        }

        public double getLoad()
        {
            return 0;
        }

        public bool getRunning()
        {
            return run;
        }

        public bool isGroup(string group)
        {
            return false;
        }

        public string[] getGroupArray()
        {
            return new string[] {};
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

        public void processingJob(Service Service, Job job)
        {
            string id = null;
            if (job.receiveResult)
            {
                id = transaction.createTransaction(job);
                if (id == null)
                {
                    job.returnResult(this, false);
                    return;
                }
            }

            job.addProcessor();

            sendMessage(PacketProcessor.createQueryPacket(id, job));
        }

        #endregion

        public PacketEncoder getPacketEncoder()
        {
            return packetEncoder;
        }

        public PacketDecoder getPacketDecoder()
        {
            return packetDecoder;
        }

        protected void disconnect()
        {
            run = false;

            packetBuffer.Dispose();
            packetBuffer = null;

            Parallel.ForEach(transaction.Dispose(), item => { item.getJob().returnResult(this, false); });
            transaction = null;
            //할당된 작업 해제 등등
        }

        protected void readCallback(IAsyncResult ar)
        {
            int len;
            try
            {
                len = getSocket().EndReceive(ar);
            }
            catch (SocketException)
            {
                disconnect();
                return;
            }

            getPacketBuffer().write(getSocketBuffer(), 0, len);

            LinkedList<dynamic> messageList = getPacketMessageList();

            var task = new Task(() =>
                                    {
                                        Parallel.ForEach(messageList, message =>
                                                                          {
                                                                              if (
                                                                                  PacketProcessor.getPacketType(this,
                                                                                                                message) ==
                                                                                  "q")
                                                                              {
                                                                                  Parallel.Invoke(
                                                                                      () =>
                                                                                          {
                                                                                              PacketProcessor.
                                                                                                  processingPacket(
                                                                                                      this, message);
                                                                                          });
                                                                              }
                                                                              else
                                                                              {
                                                                                  Job job =
                                                                                      transaction.getTransaction(
                                                                                          (string) message.t);
                                                                                  if (job != null)
                                                                                  {
                                                                                      Parallel.Invoke(
                                                                                          () =>
                                                                                              {
                                                                                                  job.returnResult(
                                                                                                      this,
                                                                                                      message.f != true
                                                                                                          ? true
                                                                                                          : false);
                                                                                              });
                                                                                  }
                                                                              }
                                                                          });
                                    });

            getSocket().BeginReceive(getSocketBuffer(), 0, 512, SocketFlags.None, readCallback, null);

            task.Start();
        }

        private LinkedList<dynamic> getPacketMessageList()
        {
            var packetList = new LinkedList<dynamic>();

            dynamic packet;
            while ((packet = getPacketDecoder().decode(getPacketBuffer())) != null)
                packetList.AddLast(packet);

            return packetList;
        }

        public Socket getSocket()
        {
            return oSocket;
        }

        public byte[] getSocketBuffer()
        {
            return socketBuffer;
        }

        public PacketBuffer getPacketBuffer()
        {
            return packetBuffer;
        }

        public bool sendMessage(dynamic data)
        {
            PacketBuffer buffer = getPacketEncoder().encode(data);
            bool r = sendPacket(buffer);
            buffer.Dispose();

            return r;
        }

        public bool sendPacket(PacketBuffer buffer)
        {
            byte[] sendBuffer = buffer.getBytes();
            try
            {
                getSocket().BeginSendTo(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, getSocket().RemoteEndPoint,
                                        sendPacketCallback, null);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        private void sendPacketCallback(IAsyncResult ar)
        {
            getSocket().EndSendTo(ar);
        }
    }
}