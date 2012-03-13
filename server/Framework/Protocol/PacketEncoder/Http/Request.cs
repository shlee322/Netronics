using System.Collections.Generic;
using System.IO;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class Request
    {
        private string _method;
        private string _url;
        private string _version;
        private readonly Dictionary<string, string> headerDictionary = new Dictionary<string, string>();

        public static Request GetRequest(TextReader reader)
        {
            var request = new Request();
            string h = reader.ReadLine();
            int s1 = h.IndexOf(" ", System.StringComparison.Ordinal);
            int s2 = h.LastIndexOf(" ", System.StringComparison.Ordinal);
            if (s1 == -1 || s2 == -1)
                return null;
            request._method = h.Substring(0, s1);
            request._url = h.Substring(s1 + 1, s2 - s1 - 1);
            request._version = h.Substring(s2+1);
            while(true)
            {
                h = reader.ReadLine();
                if (h == null)
                    break;
                if (h == "")
                {
                    reader.ReadLine();
                    return request;
                }
                request.SetHeader(h);
            }
            return null;
        }

        public void SetHeader(string s)
        {
            int valueStartIndex = s.IndexOf(": ", System.StringComparison.Ordinal);
            if (valueStartIndex == -1)
                return;
            string key = s.Substring(0, valueStartIndex);
            string value = s.Substring(valueStartIndex + 2, s.Length - valueStartIndex - 2);
            headerDictionary.Add(key, value);
        }

        public string GetHeader(string key)
        {
            return headerDictionary[key];
        }
    }
}