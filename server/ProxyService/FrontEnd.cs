using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace ProxyService
{
    public class FrontEnd
    {
        const int MaxClientID = 30000;

        private Socket _serverSocket;
        private Client[] _client;
        private int _nextClientID;
        private Stack<int> _removeClientID;
		
        public delegate Handshake GetHandshakeInstance();

        private GetHandshakeInstance _handshakeInstance;

        private static void add(FrontEnd frontEnd)
        {
        }

        public FrontEnd()
        {
            add(this);
        }

        public void Start(int port)
        {
            _client = new Client[MaxClientID];
            _nextClientID = 0;
            _removeClientID = new Stack<int>();
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _serverSocket.BeginAccept(Accept, null);
            _serverSocket.Listen(50);
        }

        private int GetClientID()
        {
            if (_removeClientID.Count == 0)
            {
                if (_nextClientID == MaxClientID)
                    return -1;
                return _nextClientID++;
            }

            return _removeClientID.Pop();
        }

		public void SetHandshake(GetHandshakeInstance handshake)
		{
		    _handshakeInstance = handshake;
		}

        private void Accept(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);
            int id = GetClientID();
            if (id == -1)
            {
                socket.Disconnect(false);
            }
            else
            {
                _client[id] = new Client(id, socket, _handshakeInstance());
            }
            _serverSocket.BeginAccept(Accept, null);
        }

    }
}
