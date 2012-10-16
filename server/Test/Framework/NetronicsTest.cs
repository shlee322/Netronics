using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using Netronics.Channel;

namespace Netronics.Test
{
    [TestFixture]
    class NetronicsTest
    {
        private Netronics CreateNetronics(int port=17777)
        {
            Console.WriteLine("Netronics 초기화 port:{0}", port);
            var netronics = new Netronics(Properties.CreateProperties(new IPEndPoint(IPAddress.Any, port), new ChannelPipe().SetCreateChannelAction(channel => { })));
            Console.WriteLine("Netronics 서버 시작 port:{0}", port);
            netronics.Start();
            return netronics;
        }

        [Test]
        public void RunTest()
        {
            var list = new List<Netronics>();
            for (int i = 0; i < 4; i++)
                list.Add(CreateNetronics(17777 + i));
            foreach (var netronicse in list)
                netronicse.Stop();
            
        }

        [Test]
        public void ConnectTest()
        {
            var netronics = CreateNetronics();

            Console.WriteLine("클라이언트 초기화");
            var socket = new System.Net.Sockets.TcpClient();
            Console.WriteLine("클라이언트 접속");
            socket.Connect(new IPEndPoint(IPAddress.Loopback, 17777));
            Console.WriteLine("클라이언트 종료");
            socket.GetStream().Close();
            netronics.Stop();
        }

        [Test]
        public void GetEndIPPointTest()
        {
            var netronics = CreateNetronics();
            Console.WriteLine("GetEndIPPoint");
            var ipPoint = netronics.GetEndIPPoint();
            if(!ipPoint.Equals(new IPEndPoint(IPAddress.Any, 17777)))
                throw new Exception("IPEndPoint 값이 변함");
            netronics.Stop();
        }
    }
}
