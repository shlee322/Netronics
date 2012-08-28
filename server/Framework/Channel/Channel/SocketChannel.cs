using System;
using System.Net.Sockets;
using Netronics.Protocol.PacketEncoder;
using log4net;

namespace Netronics.Channel.Channel
{
    public class SocketChannel : Channel
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SocketChannel)); 

        public static SocketChannel CreateChannel(Socket socket)
        {
            return new SocketChannel(socket);
        }

        private readonly byte[] _originalPacketBuffer = new byte[512];
        private readonly Socket _socket;

        private object _tag;

        private SocketChannel(Socket socket)
        {
            _socket = socket;
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

        public override void Disconnect()
        {
            try
            {
                base.Disconnect();
                _socket.BeginDisconnect(false, ar => Disconnected(), null);
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
            catch(ObjectDisposedException)
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