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

        public void Connected(IChannel channel)
        {
            channelLock.EnterWriteLock();
            _channels.AddLast(channel);
            channelLock.ExitWriteLock();
        }

        public void Disconnected(IChannel channel)
        {
            channelLock.EnterWriteLock();
            _channels.Remove(channel);
            channelLock.ExitWriteLock();
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            channelLock.EnterReadLock();
            foreach (var c in _channels)
                c.SendMessage(message);
            channelLock.ExitReadLock();
        }
    }
}
