using System;
using System.IO;
using System.Text.RegularExpressions;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Template.Http
{
    class StaticUriHandler : IUriHandler
    {
        private readonly Regex _rx;
        private readonly string _path;

        public StaticUriHandler(string uri, string path)
        {
            _rx = new Regex(uri);
            _path = path;
        }

        public string GetUri()
        {
            return _rx.ToString();
        }

        public bool IsMatch(Request request)
        {
            return _rx.IsMatch(request.GetPath());
        }

        public void Handle(IChannel channel, Request request)
        {
            var response = new Response();
            try
            {
                var reader =
                    new StreamReader(new FileStream(string.Format(_path, _rx.Split(request.GetPath())), FileMode.Open, FileAccess.Read));
                response.SetContent(reader.ReadToEnd());
                reader.Close();
            }
            catch (Exception)
            {
                response.Status = 404;
            }

            channel.SendMessage(response);
        }
    }
}
