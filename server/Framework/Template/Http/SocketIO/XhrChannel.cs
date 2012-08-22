using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Netronics.Template.Http.SocketIO
{
    class XhrChannel : IChannel
    {
        private string _host;
        private HttpContact _contact;
        private readonly ConcurrentQueue<string> _sendQueue = new ConcurrentQueue<string>();
        private HttpContact _liveContact;
        private IChannelHandler _handler;

        public XhrChannel(string host)
        {
            _host = host + ":0";
        }

        public void Connect()
        {
        }

        public void Disconnect()
        {
        }

        public void SendMessage(dynamic message)
        {
            _sendQueue.Enqueue(message);

            lock (this)
            {
                if (_liveContact == null) return;

                var contact = _liveContact;
                _liveContact = null;
                Send(contact);
            }
        }

        public object SetTag(object tag)
        {
            return null;
        }

        public object GetTag()
        {
            return null;
        }

        public void SetConfig(string name, object value)
        {
        }

        public object GetConfig(string name)
        {
            return null;
        }

        public override string ToString()
        {
            return _host;
        }

        public void Receive(MemoryStream lowPostData)
        {
            ThreadPool.QueueUserWorkItem((o) =>
                                             {
                                                 if (lowPostData != null)
                                                     _handler.MessageReceive(new MessageContext(this, lowPostData.ToArray()));
                                             });
        }

        public void Send(HttpContact contact)
        {
            string o;
            if(_sendQueue.TryDequeue(out o) && o != null)
            {
                contact.GetResponse().SetContent(o);
                contact.GetChannel().SendMessage(contact.GetResponse());
                return;
            }

            _liveContact = contact;
        }

        public IChannelHandler GetHandler()
        {
            return _handler;
        }

        public IChannelHandler SetHandler(IChannelHandler handler)
        {
            _handler = handler;
            return _handler;
        }
    }
}
