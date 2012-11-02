using System.Collections.Generic;
using System.Threading;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace ChatServer
{
    class Handler : IChannelHandler
    {
        ReaderWriterLockSlim channelLock = new ReaderWriterLockSlim();
        readonly LinkedList<IChannel> _channels = new LinkedList<IChannel>();

        public void Connected(IReceiveContext context)
        {
            context.GetChannel().SetTag(false);
            channelLock.EnterWriteLock();
            _channels.AddLast(context.GetChannel());
            channelLock.ExitWriteLock();
            SendMessage("hi! " + context.GetChannel());
            context.GetChannel().SendMessage("your name\r\n>");
        }

        public void Disconnected(IReceiveContext context)
        {
            channelLock.EnterWriteLock();
            _channels.Remove(context.GetChannel());
            channelLock.ExitWriteLock();
            SendMessage(context.GetChannel() + " 88" + "\r\n> ");
        }

        public void MessageReceive(IReceiveContext context)
        {
            if (context.GetChannel().GetTag() is bool)
            {
                context.GetChannel().SetTag(context.GetMessage());
                SendMessage(context.GetChannel() + " name is " + context.GetChannel().GetTag() + "\r\n> ");
                return;
            }
            SendMessage(context.GetChannel().GetTag() + " : " + context.GetChannel() + "> ");
        }

        public void SendMessage(string message)
        {
            channelLock.EnterReadLock();
            foreach (var c in _channels)
                c.SendMessage(message);
            channelLock.ExitReadLock();
        }
    }
}
