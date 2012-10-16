using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;

namespace Netronics.Mobile
{
    public class Mobile
    {
        private Netronics _netronics;
        private readonly int _port;
        private readonly ChannelPipe _channelPipe;
        private Dictionary<string, Commend> _cmd; 

        public Mobile(int port, X509Certificate cert)
        {
            _port = port;
            _channelPipe = new ChannelPipe(this, cert);
            _cmd = new Dictionary<string, Commend>();

            Connected = client => { };
            Disconnected = client => { };
        }

        public bool UseAuth { get; set; }

        public void Run()
        {
            foreach (var commend in _cmd)
                commend.Value.Sort();            

            _netronics = new Netronics(Properties.CreateProperties(new IPEndPoint(IPAddress.Any, _port), _channelPipe));
            _netronics.Start();
        }

        public Action<Client> Connected { get; set; }
        public Action<Client> Disconnected { get; set; }

        public void On(string type, Action<Request> action, int minVer = 0)
        {
            var ex = _cmd.ContainsKey(type);
            var cmd = ex ? _cmd[type] : new Commend(type);
            cmd.AddCmd(action, minVer);

            if (!ex)
                _cmd.Add(type, cmd);
        }

        public void Call(Request request)
        {
            if (!_cmd.ContainsKey(request.Type))
                return;
            _cmd[request.Type].Run(request);
        }
    }
}
