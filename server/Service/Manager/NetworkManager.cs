using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Service.Manager
{
    class NetworkManager
    {
        private List<Network> _networks = new List<Network>();
        public NetworkManager(string path)
        {
            var data = JArray.Parse(File.ReadAllText(Path.Combine(path, "network.ns")));
            foreach (dynamic row in data)
            {
                _networks.Add(new Network((string)row.service[0], (string)row.service[1], IPAddress.Parse((string)row.subnet).GetAddressBytes(), IPAddress.Parse((string)row.mask).GetAddressBytes()));
            }
        }

        public List<Network> GetNetworks(Service service)
        {
            var list = new List<Network>();

            foreach (Network row in _networks)
            {
                if(row.Check(service))
                {
                    list.Add(row);
                }
            }

            return list;
        }
    }
}
