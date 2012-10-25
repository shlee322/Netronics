using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Netronics;
using Netronics.Channel;

namespace SslServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Properties를 생성합니다. Properties는 Netronics의 각종 설정을 의미합니다.
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Any, 7777), // 클라이언트를 받을 아이피와 포트를 설정합니다.
                                                         new ChannelPipe(new X509Certificate2("server.pfx", "asdf")));

            var netronics = new Netronics.Netronics(properties); // 위에서 만든 Properties를 바탕으로 Netronics 객체를 생성합니다.
            netronics.Start(); // 서버를 가동합니다.
        }
    }
}
