using System.Collections.Generic;

namespace Netronics.PacketEncoder.Http
{
    public class Request
    {
        private Dictionary<string, string> headerDictionary = new Dictionary<string, string>(); 

        public void SetHeader(string key, string value)
        {
            value = value.TrimStart(' ');
            headerDictionary.Remove(key);
            headerDictionary.Add(key, value);
        }

        public string GetHeader(string key)
        {
            return headerDictionary[key];
        }
    }
}
