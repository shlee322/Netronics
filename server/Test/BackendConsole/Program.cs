using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace BackendConsole
{
    class Program
    {
        static protected System.Net.Sockets.TcpClient client;
        static protected Netronics.BSONEncoder encoder = new Netronics.BSONEncoder();
        static protected Netronics.BSONDecoder decoder = new Netronics.BSONDecoder();

        static protected JsonSerializer serializer = new JsonSerializer();

        static protected Netronics.PacketBuffer buffer;

        static protected byte[] socketBuffer = new byte[1024];


        static void Main(string[] args)
        {
            while (true)
            {
                System.Console.Write("#");
                string line = System.Console.ReadLine();
                if (line.Length > 7 && line.Substring(0, 7).Equals("server "))
                {
                    string[] arg = line.Split(' ');
                    if (arg.Length != 3)
                    {
                        System.Console.WriteLine("[server IP Port] 형식으로 입력해주세요.");
                        continue;
                    }

                    buffer = new Netronics.PacketBuffer();
                    client = new System.Net.Sockets.TcpClient(arg[1], Convert.ToInt32(arg[2]));
                    client.Client.BeginReceive(socketBuffer, 0, 1024, System.Net.Sockets.SocketFlags.None, new AsyncCallback(read), null);

                    continue;
                }

                if (line == "test1")
                {
                    //while (true)
                    for(int i=0; i<100000000; i++)
                    {
                        send(encoder.encode(JObject.Parse("{\"v\":\"1\", \"t\":\"test\", \"y\":\"q\", \"m\":{\"type\":\"test\", \"time\":\"" + DateTime.Now.Ticks + "\"}}")));
                        System.Threading.Thread.Sleep(0);
                    }
                    continue;
                }

                if (line == "test2")
                {
                    //while (true)
                    for (int i = 0; i < 10; i++)
                    {
                        send(encoder.encode(JObject.Parse("{\"v\":\"1\", \"t\":\"test\", \"y\":\"q\", \"m\":{\"type\":\"test\", \"time\":\"" + DateTime.Now.Ticks + "\"}}")));
                        System.Threading.Thread.Sleep(0);
                    }
                    continue;
                }

                try
                {
                    send(encoder.encode(JObject.Parse(line)));
                }
                catch (Exception)
                {
                    System.Console.WriteLine("데이터 형식이 잘못되었습니다.");
                }
            }
        }

        static void read(IAsyncResult ar)
        {
            int len = client.Client.EndReceive(ar);
            buffer.write(socketBuffer, 0, len);

            dynamic data;
            while ((data = decoder.decode(buffer)) != null)
            {
                if (data.y == "r" && data.r.time != null)
                {
                    System.Console.WriteLine(DateTime.Now.Ticks - Convert.ToInt64((string)data.r.time));
                }
                else
                {
                    System.Console.WriteLine(data);
                }
            }
            System.Console.Write("\n#");
            client.Client.BeginReceive(socketBuffer, 0, 1024, System.Net.Sockets.SocketFlags.None, new AsyncCallback(read), null);
        }

        static void send(Netronics.PacketBuffer buffer)
        {
            byte[] data = buffer.getBytes();
            client.Client.BeginSendTo(data, 0, data.Length, System.Net.Sockets.SocketFlags.None, client.Client.RemoteEndPoint, new AsyncCallback(sendto), null);
        }

        static void sendto(IAsyncResult ar)
        {
            client.Client.EndSendTo(ar);
        }
    }
}
