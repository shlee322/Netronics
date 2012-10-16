using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Service.Service.Manager
{
    class AddRemoveItem
    {
        public bool IsAdd;

        public string Service;
        public int Id;
        public byte[] Address;

        public int Port;

        public IChannel Channel;
    }
}
