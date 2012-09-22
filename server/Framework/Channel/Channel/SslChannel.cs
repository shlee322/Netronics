using System;
using System.Net.Security;
using System.Net.Sockets;
using Netronics.Protocol.PacketEncoder;
using log4net;

namespace Netronics.Channel.Channel
{
    /// <summary>
    /// Socket에다 SSL을 사용하는 Channel 클래스
    /// </summary>
    public class SslChannel : Channel
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SslChannel));

        private readonly byte[] _originalPacketBuffer = new byte[512];
        private readonly Socket _socket;
        private readonly SslStream _stream;
        private object _tag;

        public static SslChannel CreateChannel(Socket socket, System.Security.Cryptography.X509Certificates.X509Certificate certificate)
        {
            var channel = new SslChannel(socket);
            channel._stream.AuthenticateAsServer(certificate);
            return channel;
        }

        public static SslChannel CreateChannel(Socket socket, string host)
        {
            var channel = new SslChannel(socket);
            channel._stream.AuthenticateAsClient(host);
            return channel;
        }

        private SslChannel(Socket socket)
        {
            _socket = socket;
            _stream = new SslStream(new NetworkStream(socket, true));
        }

        public Socket GetSocket()
        {
            return _socket;
        }

        public override string ToString()
        {
            return _socket.RemoteEndPoint.ToString();
        }

        protected override void BeginReceive()
        {
            try
            {
                _socket.BeginReceive(_originalPacketBuffer, 0, 512, SocketFlags.None, ReadCallback, null);
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            int len;
            try
            {
                len = _socket.EndReceive(ar);
                if (len == 0)
                    throw new SocketException();
            }
            catch (SocketException)
            {
                Disconnect();
                return;
            }
            catch(ObjectDisposedException)
            {
                Disconnect();
                return;
            }

            ReceivePacket(_originalPacketBuffer, len);
        }

        protected override void Disconnecting()
        {
            try
            {
                base.Disconnecting();
                _socket.BeginDisconnect(false, ar => Scheduler.QueueWorkItem(GetHashCode(),
                                                                             Disconnected), null);
            }
            catch (ObjectDisposedException e)
            {
                Log.Error("Disconnect가 여러번 호출 됬습니다.", e);
            }
        }

        protected override void Disconnected()
        {
            base.Disconnected();
            _socket.Dispose();
        }

        public override void SendMessage(dynamic message)
        {
            PacketBuffer buffer = ((IPacketEncoder)GetConfig("encoder")).Encode(this, message);

            if (buffer == null)
                return;
            byte[] o = buffer.GetBytes();
            buffer.Dispose();

            try
            {
                _socket.BeginSend(o, 0, o.Length, SocketFlags.None, SendCallback, null);
            }
            catch (SocketException)
            {
            }
        }

        public override object SetTag(object tag)
        {
            _tag = tag;
            return _tag;
        }

        public override object GetTag()
        {
            return _tag;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                _socket.EndSend(ar);
            }
            catch (SocketException)
            {
            }
        }
    }
}
