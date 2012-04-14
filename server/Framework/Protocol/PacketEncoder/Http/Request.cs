using System;
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
        private readonly Dictionary<string, string> _query = new Dictionary<string, string>();
        private object _postData;

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
            if(s1 != -1 && uri.Length > s1+1)
                SetQuery(request, uri.Substring(s1+1));

            request._protocol = h.Substring(s2 + 1);

            if (GetHeaders(request, reader) == null)
                return null;

            if (request.GetMethod() == "POST" && SetPostData(request, reader) == null)
                return null;
            
            return request;
        }

        private static void SetQuery(Request request, string query)
        {
            foreach (var q in query.Split('&'))
            {
                int valueStartPoint = q.IndexOf("=", System.StringComparison.Ordinal);
                if(valueStartPoint == -1 && q.Length > valueStartPoint+1)
                    continue;
                request.AddQuery(q.Substring(0, valueStartPoint), q.Substring(valueStartPoint+1));
            }
        }

        private void AddQuery(string name, string value)
        {
            _query.Add(name, value);
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
                    return request;
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

        public string GetQuery(string key)
        {
            return _query[key];
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

        public static Request SetPostData(Request request, TextReader reader)
        {
            string data = reader.ReadToEnd();
            if (Convert.ToInt64(request.GetHeader("Content-Length")) > data.Length + 1)
                return null;

            if (request.GetHeader("Content-Type") != "application/x-www-form-urlencoded")
            {
                var postdata = new Dictionary<string, string>();
                foreach (string q in data.Split('&'))
                {
                    int valueStartPoint = q.IndexOf("=", System.StringComparison.Ordinal);
                    if (valueStartPoint == -1 && q.Length > valueStartPoint + 1)
                        continue;
                    postdata.Add(q.Substring(0, valueStartPoint), q.Substring(valueStartPoint + 1));
                }
                request._postData = postdata; 
            }

            return request;
        }

        public object GetPostData()
        {
            return _postData;
        }

        public string GetPostData(string name)
        {
            var data = _postData as Dictionary<string, string>;
            if (data == null)
                return null;
            try
            {
                return data[name];
            }
            catch
            {
            }
            return null;
        }

    }
}