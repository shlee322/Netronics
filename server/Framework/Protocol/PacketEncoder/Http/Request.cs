using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class Request
    {
        private string _method;
        private string _path;
        private string _protocol;
        private readonly Dictionary<string, string> _headerDictionary = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _query = new Dictionary<string, string>();
        private readonly Dictionary<string, object> _postData = new Dictionary<string, object>();

        public static Request GetRequest(StreamReader reader)
        {
            var request = new Request();
            request._postData.Add("FILES", new Dictionary<string, string>());

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

            if (!request.GetHeaders(reader))
                return null;

            if (request.GetMethod() == "POST" && !request.SetPostData(reader))
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

        private bool GetHeaders(TextReader reader)
        {
            string h;
            while (true)
            {
                h = reader.ReadLine();
                if (h == null)
                    break;
                if (h == "")
                    return true;
                SetHeader(h);
            }
            return false;
        }

        public void SetHeader(string s)
        {
            int valueStartIndex = s.IndexOf(": ", System.StringComparison.Ordinal);
            if (valueStartIndex == -1)
                return;
            string key = s.Substring(0, valueStartIndex).ToLower();
            string value = s.Substring(valueStartIndex + 2, s.Length - valueStartIndex - 2);
            _headerDictionary.Add(key, value);
        }

        public string GetHeader(string key)
        {
            try
            {
                return _headerDictionary[key.ToLower()];
            }
            catch
            {
            }
            return null;
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

        public bool SetPostData(TextReader reader)
        {
            string stringData = reader.ReadToEnd();
            if (Convert.ToInt64(GetHeader("Content-Length")) > System.Text.Encoding.UTF8.GetByteCount(stringData) + 1)
                return false;

            LoadPostData(GetHeader("Content-Type"), stringData);
            return true;
        }

        private void LoadPostData(string contentType, string stringData, string name = null)
        {
            if (contentType == null)
            {
                if(name != null)
                    _postData.Add(name, stringData);
            }
            else if (contentType == "application/x-www-form-urlencoded")
            {
                foreach (string q in stringData.Split('&'))
                {
                    int valueStartPoint = q.IndexOf("=", System.StringComparison.Ordinal);
                    if (valueStartPoint == -1 && q.Length > valueStartPoint + 1)
                        continue;
                    _postData.Add(q.Substring(0, valueStartPoint), q.Substring(valueStartPoint + 1));
                }
            }
            else if (contentType.StartsWith("multipart/form-data;"))
            {
                string p = string.Format("--{0}\r\n", GetHeader("Content-Type").Substring(GetHeader("Content-Type").IndexOf("boundary=") + 9));

                string[] data = Regex.Split(stringData, p);
                for (int i = 1; i < data.Length; i++)
                {
                    int point = data[i].IndexOf("\r\n\r\n");
                    var reader = new StringReader(data[i].Substring(0, point));
                    string line;
                    string pname = null;
                    string type = null;
                    while((line = reader.ReadLine()) != null)
                    {
                        if(line.StartsWith("Content-Disposition: "))
                        {
                            pname = line.Substring(line.IndexOf("name") + 6);
                            pname = pname.Substring(0, pname.Length - 1);
                        }
                        else if (line.StartsWith("Content-Type: "))
                        {
                            type = line.Substring(14);
                        }
                    }

                    LoadPostData(type, data[i].Substring(point + 4), pname);
                }
            }
            else if (name != null)
            {
                string tempName = Path.GetTempFileName();
                byte[] data = System.Text.Encoding.UTF8.GetBytes(stringData);
                var stream = new FileStream(tempName, FileMode.OpenOrCreate, FileAccess.Write);
                stream.Write(data, 0, data.Length);
                stream.Close();
                ((Dictionary<string, string>)_postData["FILES"]).Add(name, tempName);
            }
        }

        public Dictionary<string, string> GetFiles()
        {
            return _postData["FILES"] as Dictionary<string, string>;
        }

        public object GetPostData(string name)
        {
            try
            {
                return _postData[name];
            }
            catch (Exception)
            {
            }

            return null;
        }

        public string GetPostStringData(string name)
        {
            try
            {
                return _postData[name] as string;
            }
            catch (Exception)
            {
            }

            return null;
        }

    }
}