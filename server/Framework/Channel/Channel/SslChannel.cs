using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace Netronics.Channel.Channel
{
    public class SslChannel : Channel
    {
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

        private readonly byte[] _originalPacketBuffer = new byte[512];
        private readonly Socket _socket;
        private readonly SslStream _stream;

        private object _tag;

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
                _stream.BeginRead(_originalPacketBuffer, 0, 512, ReadCallback, null);
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
                len = _stream.EndRead(ar);
                if (len == 0)
                    throw new SocketException();
            }
            catch (SocketException)
            {
                ThreadPool.QueueUserWorkItem((o) => Disconnect());
                return;
            }

            ReceivePacket(_originalPacketBuffer, len);
        }

        public override void Disconnect()
        {
            _socket.BeginDisconnect(false, ar =>
            {
                if (GetHandler() != null)
                    GetHandler().Disconnected(this);
                Disconnected();
            }, null);
        }

        protected override void Disconnected()
        {
            base.Disconnected();
            _socket.Dispose();
        }



        public override void SendMessage(dynamic message)
        {
            PacketBuffer buffer = GetProtocol().GetEncoder().Encode(this, message);

            if (buffer == null)
                return;
            byte[] o = buffer.GetBytes();
            buffer.Dispose();

            try
            {
                _stream.BeginWrite(o, 0, o.Length, SendCallback, null);
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
