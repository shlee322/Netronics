using System;
using System.Net.Sockets;
using Netronics.Protocol;

namespace Netronics.Channel
{
    public class SocketChannel : IChannel
    {
        private readonly ChannelFlag _flag;

        private readonly byte[] _originalPacketBuffer = new byte[512];
        private readonly PacketBuffer _packetBuffer = new PacketBuffer();
        private readonly Socket _socket;

        private SocketChannel(Socket socket, ChannelFlag flag)
        {
            _socket = socket;
            _flag = flag;
        }

        public static SocketChannel CreateChannel(Socket socket, ChannelFlag flag)
        {
            return new SocketChannel(socket, flag);
        }

        private IProtocol GetProtocol()
        {
            var protocol = (IProtocol) _flag[ChannelFlag.Flag.Protocol];
            return protocol.GetHandShake() ?? protocol;
        }

        private IChannelHandler GetHandler()
        {
            return (IChannelHandler) _flag[ChannelFlag.Flag.Handler];
        }

        private bool GetParallel()
        {
            return (bool) _flag[ChannelFlag.Flag.Parallel];
        }

        public void Connect()
        {
            if (GetHandler() != null)
                GetHandler().Connected(this);

            BeginReceive();
        }

        public void Disconnect()
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
                _socket.BeginSend(o, 0, o.Length, SocketFlags.None, ar => { }, null);
            }
            catch (SocketException)
            {
            }
        }
    }
}