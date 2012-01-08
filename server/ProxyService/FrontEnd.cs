using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ProxyService
{
    class FrontEnd
    {
        const int MAX_CLIENT_ID = 30000;

        private Socket serverSocket;
        private Client[] client;
        private int nextClientID;
        private Stack<int> removeClientID;
		
		private Handshake handshake;

        static void add()
        {
        }


        public FrontEnd(int port)
        {
            this.client = new Client[MAX_CLIENT_ID];
            this.nextClientID = 0;
            this.removeClientID = new Stack<int>();
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void start()
        {
            this.serverSocket.BeginAccept(new AsyncCallback(this.accept), null);
            this.serverSocket.Listen(50);
        }

        private int getClientID()
        {
            if (this.removeClientID.Count == 0)
            {
                if (this.nextClientID == MAX_CLIENT_ID)
                    return -1;
                return this.nextClientID++;
            }

            return this.removeClientID.Pop();
        }
		
		public void setHandshake(Handshake handshake)
		{
			this.handshake = handshake;
		}

        private void accept(IAsyncResult ar)
        {
            Socket socket = this.serverSocket.EndAccept(ar);
            int id = this.getClientID();
            if (id == -1)
            {
                socket.Disconnect(false);
            }
            else
            {
                this.client[id] = new Client(id, socket, this.handshake);
            }
            this.serverSocket.BeginAccept(new AsyncCallback(this.accept), null);
        }

    }
}
