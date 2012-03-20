using System.Collections.Generic;
using System.Text;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class HttpEncoder : IPacketEncoder
    {
        private static readonly Dictionary<int, string> StatusDictionary = new Dictionary<int, string>();

        static HttpEncoder()
        {
            StatusDictionary.Add(100, "Continue");
            StatusDictionary.Add(101, "Switching Protocols");
            StatusDictionary.Add(102,"Processing");
            StatusDictionary.Add(103,"Checkpoint");
            StatusDictionary.Add(122,"Request-URI too long");
            StatusDictionary.Add(200,"OK");
            StatusDictionary.Add(201,"Created");
            StatusDictionary.Add(202,"Accepted");
            StatusDictionary.Add(203,"Non-Authoritative Information");
            StatusDictionary.Add(204,"No Content");
            StatusDictionary.Add(205,"Reset Content");
            StatusDictionary.Add(206,"Partial Content");
            StatusDictionary.Add(207,"Multi-Status");
            StatusDictionary.Add(208,"Already Reported");
            StatusDictionary.Add(226,"IM Used");
            StatusDictionary.Add(300,"Multiple Choices");
            StatusDictionary.Add(301,"Moved Permanently");
            StatusDictionary.Add(302,"Found");
            StatusDictionary.Add(303,"See Other");
            StatusDictionary.Add(304,"Not Modified");
            StatusDictionary.Add(305,"Use Proxy");
            StatusDictionary.Add(306,"Switch Proxy");
            StatusDictionary.Add(307,"Temporary Redirect");
            StatusDictionary.Add(308,"Resume Incomplete");
            StatusDictionary.Add(400,"Bad Request");
            StatusDictionary.Add(401,"Unauthorized");
            StatusDictionary.Add(402,"Payment Required");
            StatusDictionary.Add(403,"Forbidden");
            StatusDictionary.Add(404,"Not Found");
            StatusDictionary.Add(405,"Method Not Allowed");
            StatusDictionary.Add(406,"Not Acceptable");
            StatusDictionary.Add(407,"Proxy Authentication Required");
            StatusDictionary.Add(408,"Request Timeout");
            StatusDictionary.Add(409,"Conflict");
            StatusDictionary.Add(410,"Gone");
            StatusDictionary.Add(411,"Length Required");
            StatusDictionary.Add(412,"Precondition Failed");
            StatusDictionary.Add(413,"Request Entity Too Large");
            StatusDictionary.Add(414,"Request-URI Too Long");
            StatusDictionary.Add(415,"Unsupported Media Type");
            StatusDictionary.Add(416,"Requested Range Not Satisfiable");
            StatusDictionary.Add(417,"Expectation Failed");
            StatusDictionary.Add(418,"I'm a teapot");
            StatusDictionary.Add(420,"Enhance Your Calm");
            StatusDictionary.Add(422,"Unprocessable Entity");
            StatusDictionary.Add(423,"Locked");
            StatusDictionary.Add(424,"Failed Dependency");
            StatusDictionary.Add(425,"Unordered Collection");
            StatusDictionary.Add(426,"Upgrade Required");
            StatusDictionary.Add(428,"Precondition Required");
            StatusDictionary.Add(429,"Too Many Requests");
            StatusDictionary.Add(431,"Request Header Fields Too Large");
            StatusDictionary.Add(444,"No Response");
            StatusDictionary.Add(449,"Retry With");
            StatusDictionary.Add(450,"Blocked by Windows Parental Controls");
            StatusDictionary.Add(499,"Client Closed Request");
            StatusDictionary.Add(500,"Internal Server Error");
            StatusDictionary.Add(501,"Not Implemented");
            StatusDictionary.Add(502,"Bad Gateway");
            StatusDictionary.Add(503,"Service Unavailable");
            StatusDictionary.Add(504,"Gateway Timeout");
            StatusDictionary.Add(505,"HTTP Version Not Supported");
            StatusDictionary.Add(506,"Variant Also Negotiates");
            StatusDictionary.Add(507,"Insufficient Storage");
            StatusDictionary.Add(508,"Loop Detected");
            StatusDictionary.Add(509,"Bandwidth Limit Exceeded");
            StatusDictionary.Add(510,"Not Extended");
            StatusDictionary.Add(511,"Network Authentication Required");
            StatusDictionary.Add(598,"Network read timeout error");
            StatusDictionary.Add(599,"Network connect timeout error");
        }

        #region IPacketEncoder Members

        public PacketBuffer Encode(IChannel channel, dynamic message)
        {
            var response = message as Response;
            if (response == null)
                return null;

            var builder = new StringBuilder();
            builder.Append("HTTP/");
            builder.Append(response.Protocol);
            builder.Append(" ");
            builder.Append(response.Status);
            builder.Append(" ");
            builder.AppendLine(StatusDictionary[response.Status]);
            builder.AppendFormat("Content-Type: {0}\r\n", response.ContentType);

            string content = response.GetContent();
            builder.AppendFormat("Content-Length: {0}\r\n\r\n", content.Length);

            var buffer = new PacketBuffer();

            buffer.WriteBytes(Encoding.UTF8.GetBytes(builder.ToString()));
            buffer.WriteBytes(Encoding.UTF8.GetBytes(content));

            return buffer;
        }

        #endregion

    }
}