﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using Encoding = System.Text.Encoding;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class Response
    {
        private int _code = 200;
        private object _content = new StringBuilder();
        private string _protocol =  "1.0";
        private string _contentType = "text/html";
        private readonly StringBuilder _header = new StringBuilder();

        private static readonly string Server = "Netronics/" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public Response()
        {
            _header.AppendLine("Date: " + DateTime.Now.ToUniversalTime().ToString("ddd, d MMM yyyy hh:mm:ss UTC"));
            _header.AppendLine("Server: " + Server);
            _header.AppendLine("Accept-Ranges: bytes");
        }

        public int Status
        {
            set { _code = value; }
            get { return _code; }
        }

        public string Protocol
        {
            set { _protocol = value; }
            get { return _protocol; }
        }

        public string ContentType
        {
            set { _contentType = value; }
            get { return _contentType; }
        }

        public StringBuilder GetHeader()
        {
            return _header;
        }

        public void SetContent(string content)
        {
            _content = new StringBuilder(content);
        }

        public void SetContent(byte[] content)
        {
            _content = new MemoryStream(content);
        }

        public void SetContent(Stream stream)
        {
            _content = new MemoryStream();
            stream.CopyTo((Stream)_content);
        }

        public object GetContent()
        {
            return _content;
        }

        public StringBuilder GetContentStringBuilder()
        {
            return _content as StringBuilder;
        }
        /*
        public void SetTemplate(string file, object model=null, Encoding encoding = null)
        {
            SetTemplate<object>(file, model, encoding);
        }
        
        public void SetTemplate<T>(string file, T model, Encoding encoding = null)
        {
            var streamReader = new StreamReader(file, encoding ?? Encoding.UTF8);
            _content = new StringBuilder(Razor.Parse(streamReader.ReadToEnd(), model));
            streamReader.Close();
        }*/
    }
}