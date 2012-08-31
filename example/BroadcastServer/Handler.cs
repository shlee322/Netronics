using System;
using System.Collections.Generic;
using System.Threading;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace BroadcastServer
{
    class Handler : IChannelHandler
    {
        readonly ReaderWriterLockSlim _channelLock = new ReaderWriterLockSlim();
        readonly LinkedList<IChannel> _channels = new LinkedList<IChannel>();

        public void Connected(IReceiveContext context)
        {
            _channelLock.EnterWriteLock();
            _channels.AddLast(context.GetChannel());
            _channelLock.ExitWriteLock();
        }

        public void Disconnected(IReceiveContext context)
        {
            _channelLock.EnterWriteLock();
            _channels.Remove(context.GetChannel());
            _channelLock.ExitWriteLock();
        }

        public void MessageReceive(IReceiveContext context)
        {
            _channelLock.EnterReadLock();
            foreach (var c in _channels)
                c.SendMessage(context.GetMessage());
            _channelLock.ExitReadLock();
        }
    }
}
