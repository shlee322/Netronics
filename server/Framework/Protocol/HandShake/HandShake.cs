using System;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncryptor;

namespace Netronics.Protocol.HandShake
{
    class HandShake : IProtocol, IChannelHandler, IPacketEncoder, IPacketDecoder
    {
        public static void SetHandShake(IChannel channel, IStep step, IProtocol handShakeEncryptor = null)
        {
            new HandShake(channel, step, handShakeEncryptor);
        }

        private IStep _step;
        private readonly IProtocol _encryptor;
        private readonly IChannel _channel;
        private readonly IProtocol _protocol;
        private readonly IChannelHandler _handler;

        private HandShake(IChannel channel, IStep step, IProtocol encryptor)
        {
            if(!(channel is IKeepProtocolChannel))
                throw new InvalidCastException("HandShake에 사용하는 Channel은 IKeepProtocolChannel을 상속하는 클래스여야 합니다.");
            if (!(channel is IKeepHandlerChannel))
                throw new InvalidCastException("HandShake에 사용하는 Channel은 IKeepHandlerChannel을 상속하는 클래스여야 합니다.");

            _channel = channel;
            _step = step;
            _encryptor = encryptor;
            _protocol = ((IKeepProtocolChannel) channel).GetProtocol();
            _handler = ((IKeepHandlerChannel) channel).GetHandler();

            ((IKeepProtocolChannel) channel).SetProtocol(this);
            ((IKeepHandlerChannel) channel).SetHandler(this);
        }

        public IPacketEncryptor GetEncryptor()
        {
            return _encryptor == null ? null : _encryptor.GetEncryptor();
        }

        public IPacketDecryptor GetDecryptor()
        {
            return _encryptor == null ? null : _encryptor.GetDecryptor();
        }

        public IPacketEncoder GetEncoder()
        {
            return this;
        }

        public IPacketDecoder GetDecoder()
        {
            return this;
        }

        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            return _step.Encode(channel, data);
        }

        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            return _step.Decode(channel, buffer);
        }

        public void Connected(IChannel channel)
        {
            _step.Start(this, channel);
        }

        public void Disconnected(IChannel channel)
        {
            _step.End(this, channel);
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            _step.MessageReceive(this, channel, message);
        }

        public void NextStep(IStep step)
        {
            _step.End(this, _channel);
            _step = step;
            _step.Start(this, _channel);
        }

        public void EndHandShake()
        {
            _step.End(this, _channel);
            ((IKeepProtocolChannel)_channel).SetProtocol(_protocol);
            ((IKeepHandlerChannel)_channel).SetHandler(_handler);
            _handler.Connected(_channel);
        }
    }
}
