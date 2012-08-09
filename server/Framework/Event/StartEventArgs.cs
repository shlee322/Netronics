using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics.Event
{
    public class StartEventArgs : EventArgs
    {
        private System.Net.Sockets.Socket _socket;

        public StartEventArgs(System.Net.Sockets.Socket socket)
        {
            _socket = socket;
        }

        public System.Net.Sockets.Socket GetSocket()
        {
            return _socket;
        }
    }
}
