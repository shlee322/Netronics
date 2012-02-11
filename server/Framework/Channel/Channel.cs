using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Netronics.PacketEncoder;

namespace Netronics.Channel
{
    public class Channel
    {
        public static Channel CreateChannel(Socket socket, ChannelFlag flag)
        {
            return Channel.CreateChannel(socket, flag);
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

            lock (_packetBuffer)
            {
                _packetBuffer.Write(_originalPacketBuffer, 0, len);
            }

            Scheduler.Add(delegate
                              {
                                  while (true)
                                  {
                                      dynamic message;
                                      lock (_packetBuffer)
                                      {
                                          message = GetDecoder().Decode(this, _packetBuffer);
                                      }
                                      if (message == null)
                                          break;
                                      Scheduler.Add(() => GetHandler().MessageReceive(this, message));
                                  }
                              });
            BeginReceive();
        }

        public void SendMessage(dynamic message)
        {
            //스케줄러에 해야하지만, 일단 임시!
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
