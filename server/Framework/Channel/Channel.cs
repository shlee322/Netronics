using System;
using System.Net.Sockets;
using Netronics.PacketEncoder;

namespace Netronics.Channel
{
    public class Channel
    {
        public static Channel NewChannel(Socket socket, IPacketEncoder encoder, IPacketDecoder decoder, IChannelHandler handler)
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

            handler.Connected(this);
            _socket.BeginReceive(_originalPacketBuffer, 0, 512, SocketFlags.None, ReadCallback, null);
        }

        public void Disconnect()
        {
            _socket.BeginDisconnect(false, ar => { }, null);
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

        private void ReadCallback(IAsyncResult ar)
        {
            int len;
            try
            {
                len = _socket.EndReceive(ar);
            }
            catch (SocketException)
            {
                Disconnect();
                return;
            }
            _packetBuffer.Write(_originalPacketBuffer, 0, len);
            //스케줄러에 메시지 등록 (채널, 메시지)
            //일단 임시로 직접 처리
            dynamic message;
            while((message = _decoder.Decode(_packetBuffer)) != null)
                _handler.MessageReceive(this, message);
            //직접처리 끝
            _socket.BeginReceive(_originalPacketBuffer, 0, 512, SocketFlags.None, ReadCallback, null);
        }

        public void SendMessage(dynamic message)
        {
            //스케줄러에 해야하지만, 일단 임시!
            PacketBuffer buffer = _encoder.Encode(message);
            byte[] o = buffer.GetBytes();
            buffer.Dispose();
            _socket.BeginSend(o, 0, o.Length, SocketFlags.None, ar => { }, null);
        }
    }
}
