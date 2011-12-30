using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics
{
    public class Netronics
    {
        public enum Flag { Family, SocketType, ProtocolType, ServiceIPAddress, ServicePort, PacketEncoder, PacketDecoder }

        static protected AddressFamily family = AddressFamily.InterNetwork;
        static protected SocketType socketType = SocketType.Stream;
        static protected ProtocolType protocolType = ProtocolType.Tcp;
        static protected IPAddress addr = IPAddress.Any;
        static protected int port = 0;

        static protected PacketEncoder packetEncoder = new BSONEncoder();
        static protected PacketDecoder packetDecoder = new BSONDecoder();

        static protected Serivce oSerivce;
        static protected Socket oSocket;

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
                case Flag.PacketEncoder:
                    if (value.GetType() != typeof(PacketEncoder)) break;
                    Netronics.packetEncoder = (PacketEncoder)value;
                    break;
                case Flag.PacketDecoder:
                    if (value.GetType() != typeof(PacketDecoder)) break;
                    Netronics.packetDecoder = (PacketDecoder)value;
                    break;
            }
        }

        static protected PacketEncoder getPacketEncoder()
        {
            return Netronics.packetEncoder;
        }
        static protected PacketDecoder getPacketDecoder()
        {
            return Netronics.packetDecoder;
        }

        static public void start()
        {
            if (Netronics.serivce == null)
                return;

            PacketProcessor.init(Netronics.serivce);
            Netronics.initSocket();
            Netronics.serivce.init();

            Netronics.startSocket();
            Netronics.serivce.start();
        }

        static protected void initSocket()
        {
            Netronics.oSocket = new System.Net.Sockets.Socket(Netronics.family, Netronics.socketType, Netronics.protocolType);
            Netronics.oSocket.Bind(new IPEndPoint(Netronics.addr, Netronics.port));
        }

        static protected void startSocket()
        {
            Netronics.oSocket.Listen(50);
            Netronics.oSocket.BeginAccept(new AsyncCallback(Netronics.acceptCallback), null);
        }

        static protected void acceptCallback(IAsyncResult ar)
        {
            new RemoteSerivce(Netronics.oSocket.EndAccept(ar), Netronics.getPacketEncoder(), Netronics.getPacketDecoder());
            Netronics.oSocket.BeginAccept(new AsyncCallback(Netronics.acceptCallback), null);
        }

        static public void stop()
        {
            serivce.stop();
        }

        static public void processingJob(Job job)
        {
            PacketProcessor.processingJob(job);
        }
    }
}
