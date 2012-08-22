using System;
using System.Collections.Generic;
using System.Threading;
using Netronics.Channel;

namespace Netronics.Template.Http.SocketIO
{
    public class SocketIO : ISocketIO
    {
        private readonly ReaderWriterLockSlim _clientLock = new ReaderWriterLockSlim();
        private readonly Dictionary<string, Client> _clients = new Dictionary<string, Client>();
        private readonly Random _random = new Random();
        private readonly Dictionary<string, Action<dynamic>> _event = new Dictionary<string, Action<dynamic>>();

        public IChannelHandler GetWebSocket(string[] args)
        {
            if (args.Length < 3)
                return null;
            _clientLock.EnterReadLock();
            var client = GetClient(args[1]);
            _clientLock.ExitReadLock();
            return client;
        }

        private Client GetClient(string key)
        {
            return _clients.ContainsKey(key) ? _clients[key] : null;
        }

        public IChannelHandler GetFlashSocket(string[] args)
        {
            return null;
        }

        public void XhrPolling(HttpContact contact, string[] args)
        {
            if (args.Length < 3)
                return;
            _clientLock.EnterReadLock();
            var client = GetClient(args[1]);
            _clientLock.ExitReadLock();
            if (client == null)
                return;

            if (contact.GetChannel().ToString().Substring(0, contact.GetChannel().ToString().IndexOf(":", StringComparison.Ordinal))
                != client.GetChannel().ToString().Substring(0, client.GetChannel().ToString().IndexOf(":", StringComparison.Ordinal)))
                return;

            if(!(client.GetChannel() is XhrChannel))
                client.Connected(new ConnectContext(new XhrChannel(contact.GetChannel().ToString().Substring(0, contact.GetChannel().ToString().IndexOf(":", StringComparison.Ordinal)))));

            var channel = client.GetChannel() as XhrChannel;
            contact.GetResponse().ContentType = "text/plain";
            if (contact.GetRequest().GetMethod() == "GET")
            {
                channel.Send(contact);
                contact.IsAutoSendResponse = false;
            }
            else
            {
                //POST일 경우 클라 -> 서버
                channel.Receive(contact.GetRequest().GetLowPostData());
                contact.GetResponse().SetContent("1");
            }
            

            
        }

        public void JsonpPolling(HttpContact contact, string[] args)
        {
            if (args.Length < 3)
                return;
            _clientLock.EnterReadLock();
            var client = GetClient(args[1]);
            _clientLock.ExitReadLock();

            //contact.GetResponse().GetHeader().AppendLine("Access-Control-Allow-Origin: *");
            //contact.GetResponse().GetHeader().AppendLine("Access-Control-Allow-Credentials: true");
            //response.SetContent("0::/socket.io");
        }

        private Client CreateKey(Client client)
        {
            var index = new byte[12];
            for (int i = 0; i < 3; i++)
            {
                _random.NextBytes(index);
                var key = BitConverter.ToString(index).Replace("-", "");
                if(_clients.ContainsKey(key))
                    continue;
                _clients.Add(key, client);
                client.Key = key;
                return client;
            }
            return null;
        }

        public void Handshake(HttpContact contact, string[] args)
        {
            _clientLock.EnterWriteLock();
            var client = CreateKey(new Client(contact, this));
            _clientLock.ExitWriteLock();

            if (client == null)
            {
                contact.GetResponse().Status = 503;
                return;
            }

            contact.GetResponse().ContentType = "text/plain";
            contact.GetResponse().SetContent(client.Key + ":20:15:websocket,xhr-polling");//flashsocket,htmlfile,jsonp-polling
        }

        public void On(string id, Action<dynamic> action)
        {
            _event.Add(id, action);
        }

        public void Emit(string id, dynamic o)
        {
            if (_event.ContainsKey(id))
                _event[id](o);
        }
    }
}
