using System;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;

namespace Netronics.Protocol.HandShake
{
    class HandShake : IChannelHandler, IPacketEncoder, IPacketDecoder
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
            _channel = channel;
            _step = step;
            _encryptor = encryptor;
            //_protocol = ((IKeepProtocolChannel) channel).GetProtocol();
            //_handler = ((IKeepHandlerChannel) channel).GetHandler();

            //((IKeepProtocolChannel) channel).SetProtocol(this);
            //((IKeepHandlerChannel) channel).SetHandler(this);
        }


        public IPacketEncoder GetEncoder()
        {
            return this;
        }

        public IPacketDecoder GetDecoder()
        {
            return this;
        }

        public PacketBuffer Encode(IChannel channel, object data)
        {
            return _step.Encode(channel, data);
        }

        public object Decode(IChannel channel, PacketBuffer buffer)
        {
            return _step.Decode(channel, buffer);
        }

        public void Connected(IReceiveContext context)
        {
            _step.Start(this, null/*channel*/);
        }

        public void Disconnected(IReceiveContext context)
        {
            _step.End(this, null/*channel*/);
        }

        public void MessageReceive(IReceiveContext context)
        {
            _step.MessageReceive(this, null, null);
        }

        public void NextStep(IStep step)
        {
            _step.End(this, _channel);
            _step = step;
            _step.Start(this, _channel);
        }

        public void EndHandShake()
        {
            //_step.End(this, _channel);
            //((IKeepProtocolChannel)_channel).SetProtocol(_protocol);
            //((IKeepHandlerChannel)_channel).SetHandler(_handler);
            //_handler.Connected(_channel);
        }
    }
}
