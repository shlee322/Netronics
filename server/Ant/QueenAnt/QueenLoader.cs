using System.Collections.Generic;
using System.Net;
using Netronics.Channel;

namespace Netronics.Ant.QueenAnt
{
    class QueenLoader : Loader
    {
        private int _port;
        private MasterQueen _master;
        private Network[] _networks;

        private QueenAnt _queen;

        protected override void Load()
        {
            base.Load();

            _port = LoadPort();
            _master = LoadMaster();
            _networks = LoadNetworks();

            _queen = new QueenAnt(this);
        }

        private int LoadPort()
        {
            var port = GetConfig().GetData("port");
            if (port == null)
                return 1005;
            return port.ToObject<int>();
        }

        private MasterQueen LoadMaster()
        {
            var master = GetConfig().GetData("master");
            if (master == null)
                return null;
            var host = master.Value<string>("host");
            var port = master.Value<int>("port");
            //일단 해당 기능은 차후 지원
            return null;
        }

        private Network[] LoadNetworks()
        {
            var networks = new List<Network>();
            foreach (dynamic row in GetConfig().GetData("network"))
            {
                networks.Add(new Network((string)row.ant[0], (string)row.ant[1], IPAddress.Parse((string)row.subnet).GetAddressBytes(), IPAddress.Parse((string)row.mask).GetAddressBytes()));
            }
            return networks.ToArray();
        }

        /*
        public List<Network> GetNetworks(Service.Manager.Service service)
        {
            var list = new List<Network>();

            foreach (Network row in _networks)
            {
                if (row.Check(service))
                {
                    list.Add(row);
                }
            }

            return list;
        }*/

        public int GetPort()
        {
            return _port;
        }
    }
}
