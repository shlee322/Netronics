using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Service.Manager
{
    class AddRemoveItem
    {
        public bool IsAdd;
        public Service Service;

        public IChannel Channel;
        public Services Services;
        public int Id;
        public JArray Address;

        public int Port;
    }
}
