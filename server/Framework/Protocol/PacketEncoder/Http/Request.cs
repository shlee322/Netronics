using System.Collections.Generic;
using System.IO;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class Request
    {
        private string _method;
        private string _path;
        private string _protocol;
        private readonly Dictionary<string, string> _headerDictionary = new Dictionary<string, string>();

        public static Request GetRequest(TextReader reader)
        {
            var request = new Request();
            string h = reader.ReadLine();
            if (h == null)
                return null;
            int s1 = h.IndexOf(" ", System.StringComparison.Ordinal);
            int s2 = h.LastIndexOf(" ", System.StringComparison.Ordinal);
            if (s1 == -1 || s2 == -1)
                return null;
            request._method = h.Substring(0, s1);
            string uri = h.Substring(s1 + 1, s2 - s1 - 1);
            s1 = uri.IndexOf("?", System.StringComparison.Ordinal);
            request._path = s1 == -1 ? uri : uri.Substring(0, s1);
            if(s1 != -1)
            {
                //인자 설정
            }

            request._protocol = h.Substring(s2 + 1);

            return GetHeaders(request, reader);
        }

        private static Request GetHeaders(Request request, TextReader reader)
        {
            string h;
            while (true)
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
            _headerDictionary.Add(key, value);
        }

        public string GetHeader(string key)
        {
            return _headerDictionary[key];
        }

        public string GetMethod()
        {
            return _method;
        }

        public string GetPath()
        {
            return _path;
        }

        public string GetProtocol()
        {
            return _protocol;
        }
    }
}