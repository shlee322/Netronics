using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;

namespace Netronics
{
    public class Netronics
    {
        public enum Flag { Family, SocketType, ProtocolType, ServiceIPAddress, ServicePort }

        static protected AddressFamily family = AddressFamily.InterNetwork;
        static protected SocketType socketType = SocketType.Stream;
        static protected ProtocolType protocolType = ProtocolType.Tcp;
        static protected IPAddress addr = IPAddress.Any;
        static protected int port = 0;

        static protected Serivce oSerivce;
        static protected Socket oSocket;

        static protected Dictionary<string, LinkedList<Serivce>> globalSerivceList;

        static public Serivce serivce { set { Netronics.oSerivce = value; } get { return Netronics.oSerivce; } }

        static public void setFlag(Flag flag, object value)
        {
            switch (flag)
            {
                case Flag.Family:
                    if (value.GetType() != typeof(AddressFamily)) break;
                    Netronics.family = (AddressFamily)value;
                    break;
                case Flag.SocketType:
                    if (value.GetType() != typeof(SocketType)) break;
                    Netronics.socketType = (SocketType)value;
                    break;
                case Flag.ProtocolType:
                    if (value.GetType() != typeof(ProtocolType)) break;
                    Netronics.protocolType = (ProtocolType)value;
                    break;
                case Flag.ServiceIPAddress:
                    if (value.GetType() == typeof(IPAddress))
                        Netronics.addr = (IPAddress)value;
                    if (value.GetType() == typeof(string))
                        Netronics.addr = IPAddress.Parse((string)value);
                    break;
                case Flag.ServicePort:
                    if (value.GetType() != typeof(int)) break;
                    Netronics.port = (int)value;
                    break;
            }
        }

        static public void start()
        {
            if (Netronics.serivce == null)
                return;

            Netronics.initSerivceList();

            Netronics.initSocket();

            Netronics.serivce.start();
        }

        static protected void initSerivceList()
        {
            Netronics.globalSerivceList = new Dictionary<string, LinkedList<Serivce>>();
            LinkedList<Serivce> mySerivceType = new System.Collections.Generic.LinkedList<Serivce>();
            mySerivceType.AddFirst(Netronics.serivce);
            Netronics.globalSerivceList.Add(Netronics.serivce.getSerivceName(), mySerivceType);
        }

        static protected void initSocket()
        {
            Netronics.oSocket = new System.Net.Sockets.Socket(Netronics.family, Netronics.socketType, Netronics.protocolType);
            Netronics.oSocket.Bind(new IPEndPoint(Netronics.addr, Netronics.port));
            Netronics.oSocket.BeginAccept(new AsyncCallback(Netronics.acceptCallback), null);
        }

        static protected void startSocket()
        {
            Netronics.oSocket.Listen(50);
        }

        static protected void acceptCallback(IAsyncResult ar)
        {
            Socket client = Netronics.oSocket.EndAccept(ar);
            Netronics.oSocket.BeginAccept(new AsyncCallback(Netronics.acceptCallback), null);
        }

        static public void stop()
        {
            serivce.stop();
        }

        static public void processingJob(Job job)
        {
            MemoryStream ms = new MemoryStream();
            JsonSerializer serializer = new JsonSerializer();
            BsonWriter writer = new BsonWriter(ms);
            serializer.Serialize(writer, job.message);

            LinkedList<Serivce> serivceList;

            lock (Netronics.globalSerivceList)
            {
                serivceList = Netronics.globalSerivceList[job.getSerivceName()];
            }

            if (serivceList == null)
            {
                job.callFail();
                return;
            }

            lock (serivceList)
            {
            }
        }
    }
}
