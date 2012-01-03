using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace ProxyService
{
    class Client
    {
        private int id;
        private Socket socket;
        public Client(int id, Socket socket)
        {
            this.id = id;
            this.socket = socket;
        }
    }
}
