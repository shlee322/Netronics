using System;
using System.Net.Sockets;
using System.Threading;
using Netronics.Protocol;
using Netronics.Template.Service;

namespace Netronics.Channel.Channel
{
    public class SocketChannel : IKeepProtocolChannel, IKeepHandlerChannel, IKeepParallelChannel
    {
        public static SocketChannel CreateChannel(Socket socket)
        {
            return new SocketChannel(socket);
        }

        private readonly byte[] _originalPacketBuffer = new byte[512];
        private readonly PacketBuffer _packetBuffer = new PacketBuffer();
        private readonly Socket _socket;

        private IProtocol _protocol;
        private IChannelHandler _handler;
        private bool _parallel;

        private object _tag;

        private SocketChannel(Socket socket)
        {
            _socket = socket;
        }

        public Socket GetSocket()
        {
            return _socket;
        }

        public virtual IProtocol SetProtocol(IProtocol protocol)
        {
            _protocol = protocol;
            return protocol;
        }

        public virtual IProtocol GetProtocol()
        {
            return _protocol;
        }

        public virtual IChannelHandler SetHandler(IChannelHandler handler)
        {
            _handler = handler;
            return handler;
        }

        public virtual IChannelHandler GetHandler()
        {
            return _handler;
        }

        public bool SetParallel(bool parallel)
        {
            _parallel = parallel;
            return parallel;
        }

        protected virtual bool GetParallel()
        {
            return _parallel;
        }

        public virtual void Connect()
        {
            if (GetHandler() != null)
                GetHandler().Connected(this);

            BeginReceive();
        }

        public virtual void Disconnect()
        {
            _socket.BeginDisconnect(false, ar =>
                                               {
                                                   if (GetHandler() != null)
                                                       GetHandler().Disconnected(this);
                                                   _packetBuffer.Dispose();
                                               }, null);
        }

        public override string ToString()
        {
            return _socket.RemoteEndPoint.ToString();
        }

        private void BeginReceive()
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
            }
            catch (SocketException)
            {
                ThreadPool.QueueUserWorkItem((o) => Disconnect());
                return;
            }

            _packetBuffer.Write(_originalPacketBuffer, 0, len);

            ThreadPool.QueueUserWorkItem((o) => Receive());
        }

        private void Receive()
        {
            dynamic message;

            lock (_packetBuffer)
            {
                message = GetProtocol().GetDecoder().Decode(this, _packetBuffer);
            }

            if (message == null)
            {
                BeginReceive();
                return;
            }

            if (GetParallel())
            {
                ThreadPool.QueueUserWorkItem((o) => GetHandler().MessageReceive(this, message));
                ThreadPool.QueueUserWorkItem((o) => Receive());
            }
            else
            {
                ThreadPool.QueueUserWorkItem((o) =>
                                  {
                                      GetHandler().MessageReceive(this, message);
                                      ThreadPool.QueueUserWorkItem((s) => Receive());
                                  });
            }
        }

        public void SendMessage(dynamic message)
        {
            PacketBuffer buffer = GetProtocol().GetEncoder().Encode(this, message);

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

        public object SetTag(object tag)
        {
            _tag = tag;
            return _tag;
        }

        public object GetTag()
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