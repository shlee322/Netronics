using System;
using System.Net.Sockets;

namespace Netronics
{
    public class Netronics
    {
        private readonly Properties _properties;
        private Socket _socket;
        
        public Netronics(Properties properties)
        {
            _properties = properties;
        }

        public void Start()
        {
            InitSocket();
            StartSocket();
            _properties.OnStartEvent(this, new EventArgs());
        }

        public void Stop()
        {
            _properties.OnStopEvent(this, new EventArgs());
        }

        private void InitSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(_properties.IpEndPoint);
        }

        private void StartSocket()
        {
            _socket.Listen(50);
            _socket.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _properties.ChannelFactory.NewChannel(this, _socket.EndAccept(ar));
            _socket.BeginAccept(AcceptCallback, null);
        }


        /*

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
            BroadCast,
            ThreadCount
        }

        #endregion

        private static AddressFamily _family = ;
        private static SocketType _socketType = ;
        private static ProtocolType _protocolType = ;
        private static IPAddress _addr;
        private static int _port;

        private static IPacketEncoder _packetEncoder = new BsonEncoder();
        private static IPacketDecoder _packetDecoder = new BsonDecoder();

        private static IPEndPoint _router;
        private static bool _isBroadCast;

        private static UdpClient _broadCastSocket;

        protected static Service _Service;
        protected static Socket _Socket;

        private static int _threadCount = 4;

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
                case Flag.ThreadCount:
                    if (value is int)
                        _threadCount = (int) value;
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
            Scheduler.Start(_threadCount);
            StartSocket();

            StartBroadCast();

            if (_router != null)
                ConnectionService(_router);
        }

        private static void StartBroadCast()
        {
            _broadCastSocket = new UdpClient(new IPEndPoint(_addr, 7777));
            _broadCastSocket.BeginReceive(BroadCastReceiveCallback, null);

            //브로드캐스트를 허용하면 내부망으로 자신의 서비스를 알림
            if(_isBroadCast)
            {
                //이부분을 특정 주기에 한번씩 반복 하게 만들자.
                _broadCastSocket.BeginSend(new byte[] {}, 6, new IPEndPoint(IPAddress.Broadcast, 7777),
                                           delegate(IAsyncResult ar) { }, null);
            }
        }

        private static void BroadCastReceiveCallback(IAsyncResult ar)
        {
            IPEndPoint point = null;
            byte[] data = _broadCastSocket.EndReceive(ar, ref point);

        }

        private static void ConnectionService(IPEndPoint point)
        {
            TcpClient socket = new TcpClient();
            socket.BeginConnect(point.Address, point.Port, ConnectCallback, socket);
        }

        private static void ConnectCallback(IAsyncResult ar)
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
        }*/
    }
}