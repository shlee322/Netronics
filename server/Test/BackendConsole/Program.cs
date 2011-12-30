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
                if (line.Substring(0, 7).Equals("server "))
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

                send(encoder.encode(JObject.Parse(line)));
            }
        }

        static void read(IAsyncResult ar)
        {
            int len = client.Client.EndReceive(ar);
            buffer.write(socketBuffer, 0, len);

            dynamic data;
            while ((data = decoder.decode(buffer)) != null)
            {
                System.Console.WriteLine(data);
            }

            client.Client.BeginReceive(socketBuffer, 0, 1024, System.Net.Sockets.SocketFlags.None, new AsyncCallback(read), null);
        }

        static void send(Netronics.PacketBuffer buffer)
        {
            client.Client.Send(buffer.getBytes());
        }
    }
}
