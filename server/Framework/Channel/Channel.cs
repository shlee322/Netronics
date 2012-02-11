using System;
using System.Net.Sockets;
using Netronics.PacketEncoder;

namespace Netronics.Channel
{
    public class Channel
    {
        public static Channel CreateChannel(Socket socket, ChannelFlag flag)
        {
            return CreateChannel(socket, flag);
        }
		
        private Socket _socket;
		private ChannelFlag _flag;
		
        private readonly byte[] _originalPacketBuffer = new byte[512];
        private readonly PacketBuffer _packetBuffer = new PacketBuffer();

        private Channel(Socket socket, ChannelFlag flag)
        {
            _socket = socket;
			_flag = flag;

            if (GetHandler() != null)
                GetHandler().Connected(this);

            BeginReceive();
        }

        private IChannelHandler GetHandler()
        {
            return (IChannelHandler)_flag[ChannelFlag.Flag.Handler];
        }

        private IPacketEncoder GetEncoder()
        {
            return (IPacketEncoder)_flag[ChannelFlag.Flag.Encoder];
        }

        private IPacketDecoder GetDecoder()
        {
            return (IPacketDecoder)_flag[ChannelFlag.Flag.Decoder];
        }

        private bool GetParallel()
        {
            return (bool) _flag[ChannelFlag.Flag.Parallel];
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
			catch(SocketException)
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
                message = GetDecoder().Decode(this, _packetBuffer);
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
            PacketBuffer buffer = GetEncoder().Encode(this, message);
            
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
