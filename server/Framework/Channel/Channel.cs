using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Netronics.PacketEncoder;

namespace Netronics.Channel
{
    public class Channel
    {
        public static Channel CreateChannel(Socket socket, IPacketEncoder encoder, IPacketDecoder decoder, IChannelHandler handler)
        {
            return new Channel(socket, encoder, decoder, handler);
        }

        private Socket _socket;
        private IPacketEncoder _encoder;
        private IPacketDecoder _decoder;
        private IChannelHandler _handler;
        private readonly byte[] _originalPacketBuffer = new byte[512];
        private readonly PacketBuffer _packetBuffer = new PacketBuffer();

        private Channel(Socket socket, IPacketEncoder encoder, IPacketDecoder decoder, IChannelHandler handler)
        {
            _socket = socket;
            _encoder = encoder;
            _decoder = decoder;
            _handler = handler;

            if(_handler != null)
                _handler.Connected(this);

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

        public Channel SetPacketEncoder(IPacketEncoder encoder)
        {
            _encoder = encoder;
            return this;
        }

        public Channel SetPacketDecoder(IPacketDecoder decoder)
        {
            _decoder = decoder;
            return this;
        }

        public Channel SetChannelHandler(IChannelHandler channelHandler)
        {
            _handler = channelHandler;
            return this;
        }

        public IChannelHandler GetChannelHandler()
        {
            return _handler;
        }

        public IPacketEncoder GetPacketEncoder()
        {
            return _encoder;
        }

        public IPacketDecoder GetPacketDecoder()
        {
            return _decoder;
        }

        private void BeginReceive()
        {
            _socket.BeginReceive(_originalPacketBuffer, 0, 512, SocketFlags.None, ReadCallback, null);
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
            _socket.BeginSend(o, 0, o.Length, SocketFlags.None, ar => { }, null);
        }
    }
}
