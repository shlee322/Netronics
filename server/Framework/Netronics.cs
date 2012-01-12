using System;
using System.Net;
using System.Net.Sockets;

namespace Netronics
{
    public class Netronics
    {
        #region Flag enum

        public enum Flag
        {
            Family,
            SocketType,
            ProtocolType,
            ServiceIpAddress,
            ServicePort,
            PacketEncoder,
            PacketDecoder,
            Router,
            BroadCast
        }

        #endregion

        private static AddressFamily _family = AddressFamily.InterNetwork;
        private static SocketType _socketType = SocketType.Stream;
        private static ProtocolType _protocolType = ProtocolType.Tcp;
        private static IPAddress _addr = IPAddress.Any;
        private static int _port;

        private static IPacketEncoder _packetEncoder = new BsonEncoder();
        private static IPacketDecoder _packetDecoder = new BsonDecoder();

        private static IPEndPoint _router;
        private static bool _isBroadCast;

        protected static Service _Service;
        protected static Socket _Socket;

        public static Service Service
        {
            set { _Service = value; }
            get { return _Service; }
        }

        public static void SetFlag(Flag flag, object value)
        {
            switch (flag)
            {
                case Flag.Family:
                    if (value is AddressFamily)
                        _family = (AddressFamily) value;
                    break;
                case Flag.SocketType:
                    if (value is SocketType)
                        _socketType = (SocketType) value;
                    break;
                case Flag.ProtocolType:
                    if (value is ProtocolType)
                        _protocolType = (ProtocolType) value;
                    break;
                case Flag.ServiceIpAddress:
                    if (value is IPAddress)
                        _addr = (IPAddress) value;
                    if (value is string)
                        _addr = IPAddress.Parse((string) value);
                    break;
                case Flag.ServicePort:
                    if (value is int)
                        _port = (int) value;
                    break;
                case Flag.PacketEncoder:
                    if (value is IPacketEncoder)
                        _packetEncoder = (IPacketEncoder) value;
                    break;
                case Flag.PacketDecoder:
                    if (value is IPacketDecoder)
                        _packetDecoder = (IPacketDecoder) value;
                    break;
                case Flag.Router:
                    if (value is IPEndPoint)
                        _router = (IPEndPoint) value;
                    break;
                case Flag.BroadCast:
                    if (value is bool)
                        _isBroadCast = (bool)value;
                    break;
            }
        }

        protected static IPacketEncoder GetPacketEncoder()
        {
            return _packetEncoder;
        }

        protected static IPacketDecoder GetPacketDecoder()
        {
            return _packetDecoder;
        }

        public static void Start()
        {
            if (Service == null)
                return;

            PacketProcessor.Init(Service);
            InitSocket();
            Service.Init();

            Service.Start();
            StartSocket();

            StartBroadCast();

            if (_router != null)
                ConnectionService(_router);
        }

        private static void StartBroadCast()
        {
        }

        private static void ConnectionService(IPEndPoint point)
        {
            TcpClient socket = new TcpClient();
            socket.BeginConnect(point.Address, point.Port, RequestCallback, socket);
        }

        private static void RequestCallback(IAsyncResult ar)
        {
            TcpClient socket = (TcpClient) ar.AsyncState;
            socket.EndConnect(ar);

            RemoteService service = new RemoteService(socket.Client, GetPacketEncoder(), GetPacketDecoder());
            if (Service.GetRunning())
                service.ProcessingJob(Service, ServiceJob.ServiceInfo(Service));
        }

        protected static void InitSocket()
        {
            _Socket = new Socket(_family, _socketType, _protocolType);
            _Socket.Bind(new IPEndPoint(_addr, _port));
        }

        protected static void StartSocket()
        {
            _Socket.Listen(50);
            _Socket.BeginAccept(AcceptCallback, null);
        }

        protected static void AcceptCallback(IAsyncResult ar)
        {
            RemoteService service = new RemoteService(_Socket.EndAccept(ar), GetPacketEncoder(), GetPacketDecoder());
            if(Service.GetRunning())
                service.ProcessingJob(Service, ServiceJob.ServiceInfo(Service));
            _Socket.BeginAccept(AcceptCallback, null);
        }

        public static void Stop()
        {
            Service.Stop();
        }

        public static void ProcessingJob(Job job)
        {
            PacketProcessor.ProcessingJob(job);
        }
    }
}