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
        protected Socket Socket;
        protected PacketBuffer PacketBuffer = new PacketBuffer();

        protected IPacketDecoder PacketDecoder;
        protected IPacketEncoder PacketEncoder;

        protected bool Run;
        protected byte[] SocketBuffer = new byte[512];
        protected Transaction Transaction;

        public RemoteService(Socket socket, IPacketEncoder encoder, IPacketDecoder decoder)
        {
            Socket = socket;
            PacketEncoder = encoder;
            PacketDecoder = decoder;
            Transaction = new Transaction();

            GetSocket().BeginReceive(GetSocketBuffer(), 0, 512, SocketFlags.None, ReadCallback, null);
        }

        #region Service Members

        public string GetServiceName()
        {
            return ServiceName;
        }

        public double GetLoad()
        {
            return 0;
        }

        public bool GetRunning()
        {
            return Run;
        }

        public bool IsGroup(string group)
        {
            return false;
        }

        public string[] GetGroupArray()
        {
            return new string[] {};
        }

        public void Init()
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void ProcessingJob(Service service, Job job)
        {
            string id = null;
            if (job.ReceiveResult)
            {
                id = Transaction.CreateTransaction(job);
                if (id == null)
                {
                    job.ReturnResult(this, false);
                    return;
                }
            }

            job.AddProcessor();

            SendMessage(PacketProcessor.CreateQueryPacket(id, job));
        }

        #endregion

        public IPacketEncoder GetPacketEncoder()
        {
            return PacketEncoder;
        }

        public IPacketDecoder GetPacketDecoder()
        {
            return PacketDecoder;
        }

        protected void Disconnect()
        {
            Run = false;

            PacketBuffer.Dispose();
            PacketBuffer = null;

            Parallel.ForEach(Transaction.Dispose(), item => { item.GetJob().ReturnResult(this, false); });
            Transaction = null;
            //할당된 작업 해제 등등
        }

        protected void ReadCallback(IAsyncResult ar)
        {
            int len;
            try
            {
                len = GetSocket().EndReceive(ar);
            }
            catch (SocketException)
            {
                Disconnect();
                return;
            }

            GetPacketBuffer().Write(GetSocketBuffer(), 0, len);

            LinkedList<dynamic> messageList = GetPacketMessageList();

            var task = new Task(() =>
                                    {
                                        Parallel.ForEach(messageList, message =>
                                                                          {
                                                                              if (
                                                                                  PacketProcessor.GetPacketType(this,
                                                                                                                message) ==
                                                                                  "q")
                                                                              {
                                                                                  Parallel.Invoke(
                                                                                      () =>
                                                                                          {
                                                                                              PacketProcessor.
                                                                                                  ProcessingPacket(
                                                                                                      this, message);
                                                                                          });
                                                                              }
                                                                              else
                                                                              {
                                                                                  Job job =
                                                                                      Transaction.GetTransaction(
                                                                                          (string) message.t);
                                                                                  if (job != null)
                                                                                  {
                                                                                      Parallel.Invoke(
                                                                                          () =>
                                                                                              {
                                                                                                  job.ReturnResult(
                                                                                                      this,
                                                                                                      !message.f);
                                                                                              });
                                                                                  }
                                                                              }
                                                                          });
                                    });

            GetSocket().BeginReceive(GetSocketBuffer(), 0, 512, SocketFlags.None, ReadCallback, null);

            task.Start();
        }

        private LinkedList<dynamic> GetPacketMessageList()
        {
            var packetList = new LinkedList<dynamic>();

            dynamic packet;
            while ((packet = GetPacketDecoder().Decode(GetPacketBuffer())) != null)
                packetList.AddLast(packet);

            return packetList;
        }

        public Socket GetSocket()
        {
            return Socket;
        }

        public byte[] GetSocketBuffer()
        {
            return SocketBuffer;
        }

        public PacketBuffer GetPacketBuffer()
        {
            return PacketBuffer;
        }

        public bool SendMessage(dynamic data)
        {
            PacketBuffer buffer = GetPacketEncoder().Encode(data);
            bool r = SendPacket(buffer);
            buffer.Dispose();

            return r;
        }

        public bool SendPacket(PacketBuffer buffer)
        {
            byte[] sendBuffer = buffer.GetBytes();
            try
            {
                GetSocket().BeginSendTo(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, GetSocket().RemoteEndPoint,
                                        SendPacketCallback, null);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        private void SendPacketCallback(IAsyncResult ar)
        {
            GetSocket().EndSendTo(ar);
        }
    }
}