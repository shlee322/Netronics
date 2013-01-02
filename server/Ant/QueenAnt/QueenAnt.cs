using System.Collections.Generic;
using System.Net;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.QueenAnt
{
    class QueenAnt
    {
        private static QueenAnt _instance;
        private AntConfig _config;
        private int _port;
        private Network[] _networks;

        private Ants[] _ants;
        private string[] _antNameArray; 
        private Dictionary<string, Ants> _antNames;

        public static QueenAnt GetQueenAnt()
        {
            return _instance;
        }

        public static void Init(AntConfig config)
        {
            new QueenAnt(config);
        }

        private QueenAnt(AntConfig config)
        {
            _instance = this;
            _config = config;

            _port = LoadPort();
            //LoadMaster();
            _networks = LoadNetworks();

            InitAnts();

            AntPacketHandler.Init();
        }

        private void InitAnts()
        {
            var antName = new List<string>();

            foreach (var network in _networks)
            {
                if(!antName.Exists((name)=>name==network.Service1))
                    antName.Add(network.Service1);
                if (!antName.Exists((name) => name == network.Service2))
                    antName.Add(network.Service2);
            }

            _ants = new Ants[antName.Count];
            _antNameArray = antName.ToArray();
            _antNames = new Dictionary<string, Ants>();

            for (int i = 0; i < antName.Count; i++)
            {
                var name = antName[i];
                _ants[i] = new Ants(i, name);
                _antNames.Add(name, _ants[i]);
            }

            foreach (var network in _networks)
            {
                network.Service1Id = _antNames[network.Service1].GetId();
                network.Service2Id = _antNames[network.Service2].GetId();
            }
        }

        private AntConfig GetConfig()
        {
            return _config;
        }

        private int LoadPort()
        {
            var port = GetConfig().GetData("port");
            if (port == null)
                return 1005;
            return port.ToObject<int>();
        }
        /*
        private MasterQueen LoadMaster()
        {
            var master = GetConfig().GetData("master");
            if (master == null)
                return null;
            var host = master.Value<string>("host");
            var port = master.Value<int>("port");
            //일단 해당 기능은 차후 지원
            return null;
        }*/

        private Network[] LoadNetworks()
        {
            var networks = new List<Network>();
            foreach (dynamic row in GetConfig().GetData("network"))
            {
                networks.Add(new Network((string)row.ant[0], (string)row.ant[1], IPAddress.Parse((string)row.subnet).GetAddressBytes(), IPAddress.Parse((string)row.mask).GetAddressBytes()));
            }
            return networks.ToArray();
        }

        public int GetPort()
        {
            return _port;
        }

        public string[] GetAntNameArray()
        {
            return _antNameArray;
        }

        public Ants GetAnts(int id)
        {
            return _ants[id];
        }

        public IEnumerable<Network> GetNetwork()
        {
            return _networks;
        }

        public IEnumerable<JObject> GetNetwork(Ant ant)
        {
            var array = new List<JObject>();
            foreach (var network in GetNetwork())
            {
                if (!network.Check(ant))
                    continue;
                foreach (var targetAnts in _ants)
                {
                    if (network.Service1 != targetAnts.GetName() && network.Service2 != targetAnts.GetName())
                        continue;

                    foreach (var targetAnt in targetAnts.GetAnt())
                    {
                        if (targetAnt == ant)
                            continue;
                        var obj = new JObject();
                        obj.Add("ant", targetAnts.GetId());
                        obj.Add("id", targetAnt.GetId());
                        obj.Add("host", new JArray(network.GetAddress(targetAnt)));
                        obj.Add("port", targetAnt.GetPort());
                        array.Add(obj);
                    }
                }
            }
            return array;
        }
    }
}
