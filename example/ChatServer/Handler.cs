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

        public void Connected(IChannel channel)
        {
            channel.SetTag(false);
            channelLock.EnterWriteLock();
            _channels.AddLast(channel);
            channelLock.ExitWriteLock();
            SendMessage("hi! " + channel + "\r\n");
            channel.SendMessage("your name : ");
        }

        public void Disconnected(IChannel channel)
        {
            channelLock.EnterWriteLock();
            _channels.Remove(channel);
            channelLock.ExitWriteLock();
            SendMessage(channel + " 88" + "\r\n> ");
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            if (channel.GetTag() is bool)
            {
                channel.SetTag(((string)message).Substring(0, ((string)message).LastIndexOf("\r\n", System.StringComparison.Ordinal)));
                SendMessage(channel + " name is " + channel.GetTag() + "\r\n> ");
                return;
            }
            if (message == "\r\n")
            {
                channel.SendMessage("\r\n> ");
                return;
            }
            SendMessage(channel.GetTag() + " : " + message + "> ");
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
