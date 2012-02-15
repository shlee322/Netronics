using System;
using System.Net.Sockets;
using Netronics.Protocol;

namespace Netronics.Channel
{
    public class SocketChannel : IChannel, IKeepProtocolChannel, IKeepHandlerChannel, IKeepParallelChannel
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

        private SocketChannel(Socket socket)
        {
            _socket = socket;
        }

        public virtual IProtocol SetProtocol(IProtocol protocol)
        {
            _protocol = protocol;
            return protocol;
        }

        protected virtual IProtocol GetProtocol()
        {
            return _protocol;
        }

        public virtual IChannelHandler SetHandler(IChannelHandler handler)
        {
            _handler = handler;
            return handler;
        }

        protected virtual IChannelHandler GetHandler()
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
                Scheduler.Add(Disconnect);
                return;
            }

            _packetBuffer.Write(_originalPacketBuffer, 0, len);

            Scheduler.Add(Receive);
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
                Scheduler.Add(() => GetHandler().MessageReceive(this, message));
                Scheduler.Add(Receive);
            }
            else
            {
                Scheduler.Add(() =>
                                  {
                                      GetHandler().MessageReceive(this, message);
                                      Scheduler.Add(Receive);
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
                _socket.BeginSend(o, 0, o.Length, SocketFlags.None, ar => _socket.EndSend(ar), null);
            }
            catch (SocketException)
            {
            }
        }
    }
}