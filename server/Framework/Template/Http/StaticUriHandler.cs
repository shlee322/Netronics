using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Template.Http
{
    class StaticUriHandler : IUriHandler
    {
        private static readonly Dictionary<string, string> ContentTypeDictionary = new Dictionary<string, string>();

        private readonly Regex _rx;
        private readonly string _path;

        public static void AddContentType(string contentType, string extension)
        {
            ContentTypeDictionary.Add(extension, contentType);
        }

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
            response.Protocol = request.GetProtocol();
            FileStream reader = null;
            try
            {
                reader = new FileStream(string.Format(_path, _rx.Split(request.GetPath())), FileMode.Open, FileAccess.Read);

                int index = request.GetPath().LastIndexOf(".");
                if(index != -1 && index+1 < request.GetPath().Length)
                {
                    try
                    {
                        string extension = request.GetPath().Substring(index+1);
                        response.ContentType = ContentTypeDictionary[extension];
                    }
                    catch (Exception)
                    {
                    }
                }

                if(response.ContentType.StartsWith("text/"))
                    response.SetContent(new StreamReader(reader).ReadToEnd());
                else
                    response.SetContent(reader);
            }
            catch (Exception)
            {
                response.Status = 404;
            }

            if (reader != null)
                reader.Close();

            channel.SendMessage(response);
        }

        static StaticUriHandler()
        {
            AddContentType("application/acad ", "dwg");
            AddContentType("application/clariscad","ccad");
            AddContentType("application/dxf","dxf");
            AddContentType("application/msaccess","mdb");
            AddContentType("application/msword","doc");
            AddContentType("application/octet-stream","bin");
            AddContentType("application/pdf","pdf");
            AddContentType("application/postscript","ai");
            AddContentType("application/postscript","ps");
            AddContentType("application/postscript","eps");
            AddContentType("application/rtf", "rtf");
            //AddContentType("application/vnd.ms-excel", "xls");
            //AddContentType("application/vnd.ms-powerpoint", "ppt");
            AddContentType("application/x-cdf", "cdf");
            AddContentType("application/x-csh", "csh");
            AddContentType("application/x-dvi", "dvi");
            AddContentType("application/x-javascript", "js");
            AddContentType("application/x-latex","latex");
            AddContentType("application/x-mif", "mif");
            AddContentType("application/x-msexcel", "xls");
            AddContentType("application/x-mspowerpoint", "ppt");
            AddContentType("application/x-tcl", "tcl");
            AddContentType("application/x-tex", "tex");
            AddContentType("application/x-texinfo", "texinfo");
            AddContentType("application/x-texinfo", "texi");
            AddContentType("application/x-troff", "t");
            AddContentType("application/x-troff", "tr");
            AddContentType("application/x-troff", "roff");
            AddContentType("application/x-troff-man","man");
            AddContentType("application/x-troff-me","me");
            AddContentType("application/x-troff-ms","ms");
            AddContentType("application/x-wais-source","src");
            //AddContentType("application/zip","zip");
            AddContentType("audio/basic","au");
            AddContentType("audio/basic","snd");
            AddContentType("audio/x-aiff","aif");
            AddContentType("audio/x-aiff","aiff");
            AddContentType("audio/x-aiff","aifc");
            AddContentType("audio/x-wav","wav");
            AddContentType("image/gif","gif");
            AddContentType("image/png", "png");
            AddContentType("image/ief","ief");
            AddContentType("image/jpeg","jpeg");
            AddContentType("image/jpeg","jpg");
            AddContentType("image/jpeg","jpe");
            AddContentType("image/tiff","tiff");
            AddContentType("image/tiff","tif");
            AddContentType("image/x-cmu-raster","ras");
            AddContentType("image/x-portable-anymap","pnm");
            AddContentType("image/x-portable-bitmap","pbm");
            AddContentType("image/x-portable-graymap","pgm");
            AddContentType("image/x-portable-pixmap","ppm");
            AddContentType("image/x-rgb","rgb");
            AddContentType("image/x-xbitmap","xbm");
            AddContentType("image/x-xpixmap","xpm");
            AddContentType("image/x-xwindowdump","xwd");
            AddContentType("multipart/x-gzip","gzip");
            AddContentType("multipart/x-zip","zip");
            AddContentType("text/css","css");
            AddContentType("text/html","html");
            AddContentType("text/html","htm");
            AddContentType("text/plain","txt");
            AddContentType("text/richtext","rtx");
            AddContentType("text/tab-separated- values","tsv");
            AddContentType("text/xml","xml");
            AddContentType("text/x-setext","etx");
            AddContentType("text/xsl","xsl");
            AddContentType("video/mpeg","mpeg");
            AddContentType("video/mpeg","mpg");
            AddContentType("video/mpeg","mpe");
            AddContentType("video/quicktime","qt");
            AddContentType("video/quicktime","mov");
            AddContentType("video/x-msvideo","avi");
            AddContentType("video/x-sgi-movie", "movie");
        }
    }
}
