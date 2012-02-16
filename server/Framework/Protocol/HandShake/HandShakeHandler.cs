using System;
using Netronics.Channel;
using Netronics.Protocol.PacketEncoder;

namespace Netronics.Protocol.HandShake
{
    class HandShakeHandler : IChannelHandler, IPacketEncoder, IPacketDecoder
    {
        private IProtocol substantialProtocol;
        private Func<IChannelHandler, IChannel> _substantialHandler;
        private IStep _step;

        public HandShakeHandler(Func<IChannelHandler, IChannel> substantialHandler, IStep startStep)
        {
            _substantialHandler = substantialHandler;
            _step = startStep;
        }

        //핸드쉐이크용 프로토콜
        //핸드쉐이크 완료후 프로토콜
        public void Connected(IChannel channel)
        {
            _step.Start(this, channel);
        }

        public void Disconnected(IChannel channel)
        {
            _step.End(this, channel);
        }

        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            return _step.Encode(channel, data);
        }

        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            return _step.Decode(channel, buffer);
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            _step.MessageReceive(this, channel, message);
        }

        public void NextStep(IStep step, IChannel channel)
        {
            _step.End(this, channel);
            _step = step;
            _step.Start(this, channel);
        }

        public void EndHandShake(IChannel channel)
        {
            _step.End(this, channel);
        }
    }
}
