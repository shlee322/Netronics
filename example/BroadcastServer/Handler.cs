using System;
using System.Collections.Generic;
using System.Threading;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace BroadcastServer
{
    class Handler : IChannelHandler
    {
        ReaderWriterLockSlim channelLock = new ReaderWriterLockSlim();
        readonly LinkedList<IChannel> _channels = new LinkedList<IChannel>();

        public void Connected(IReceiveContext context)
        {
            channelLock.EnterWriteLock();
            _channels.AddLast(context.GetChannel());
            channelLock.ExitWriteLock();
        }

        public void Disconnected(IReceiveContext context)
        {
            channelLock.EnterWriteLock();
            _channels.Remove(context.GetChannel());
            channelLock.ExitWriteLock();
        }

        public void MessageReceive(IReceiveContext context)
        {
            channelLock.EnterReadLock();
            foreach (var c in _channels)
                c.SendMessage(context.GetMessage());
            channelLock.ExitReadLock();
        }
    }
}
