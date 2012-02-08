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

            if(_flag[ChannelFlag.Flag.Handler].GetType == typeof(IChannelHandler))
                ((IChannelHandler)_flag[ChannelFlag.Flag.Handler]).Connected(this);

            BeginReceive();
        }
		
        public void Disconnect()
        {
            _socket.BeginDisconnect(false, ar =>
                                               {
                                                   if (_handler != null)
                                                    _handler.Disconnected(this);
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
                                          message = _decoder.Decode(_packetBuffer);
                                      }
                                      if (message == null)
                                          break;
                                      Scheduler.Add(() => _handler.MessageReceive(this, message));
                                  }
                              });
            BeginReceive();
        }

        public void SendMessage(dynamic message)
        {
            //스케줄러에 해야하지만, 일단 임시!
            PacketBuffer buffer = _encoder.Encode(message);
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
