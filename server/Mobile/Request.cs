using Newtonsoft.Json.Linq;

namespace Netronics.Mobile
{
    public class Request
    {
        public Client Client { get; set; }
        public string Type { get; set; }
        public JToken Arg { get; set; }
    }
}
