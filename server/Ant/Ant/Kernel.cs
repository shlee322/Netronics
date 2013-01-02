using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Netronics.Ant.Ant.Network;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;
using Netronics.Ant.Packet;

namespace Netronics.Ant.Ant
{
    class Kernel
    {
        private static Kernel _instance;

        private AntConfig _config;
        private IPEndPoint[] _queenIPEndPoints;

        private string _fileName;
        private string _className;

        private Assembly _assembly;
        private AntModel _localAntModel;

        private Ants[] _ants;
        private string[] _antNameArray;
        private Dictionary<string, Ants> _antNames;

        private LocalAnt _localAnt;

        private IChannel _queenChannel;


        public static Kernel GetKernel()
        {
            return _instance;
        }

        public static void Init(AntConfig config)
        {
            new Kernel(config);
        }

        private Kernel(AntConfig config)
        {
            _instance = this;
            _config = config;

            LoadQueenIPEndPoint();
            LoadAssembly();

            AntPipeline.Init();
            QueenPacketHandler.Init();
        }

        private AntConfig GetConfig()
        {
            return _config;
        }

        private void LoadQueenIPEndPoint()
        {
            var queenArray = GetConfig().GetData("queen");
            if (!(queenArray is JArray))
                throw new Exception("Queen 설정이 잘못 되었습니다.");

            _queenIPEndPoints =
                queenArray.Select(queen =>
                    new IPEndPoint(IPAddress.Parse(queen.Value<string>("host")), queen.Value<int>("port"))
                    ).ToArray();
        }

        private void LoadAssembly()
        {
            var ant = GetConfig().GetData("ant");
            _fileName = ant.Value<string>("file");
            _className = ant.Value<string>("class");

            if (_fileName == "" || _className == "")
                throw new Exception("Ant를 로드할 수 없습니다.");

            _assembly = Assembly.LoadFrom(_fileName);
            _localAntModel = _assembly.CreateInstance(_className) as AntModel;
            if (_localAntModel == null)
                throw new Exception("AntModel을 로드 할 수 없습니다.");
        }

        public IPEndPoint GetQueenIPEndPoint()
        {
            return _queenIPEndPoints[0];
        }

        public void QueenConnected(IChannel channel)
        {
            _queenChannel = channel;
            channel.SendMessage("get_ant_name_list", null);
        }

        public void InitAnts(IList list)
        {
            var antNameList = new List<string>();
            foreach (var obj in list)
            {
                antNameList.Add(obj.ToString());
            }

            _antNameArray = antNameList.ToArray();
            _ants = new Ants[_antNameArray.Length];
            _antNames = new Dictionary<string, Ants>();

            for (int i = 0; i < _antNameArray.Length; i++)
            {
                var ant = new Ants(i, _antNameArray[i]);
                _ants[i] = ant;
                _antNames.Add(_antNameArray[i], _ants[i]);
            }

            if (!_antNames.ContainsKey(_localAntModel.GetName()))
            {
                throw new Exception("Local Ant가 Queen 소속이 아닙니다.");
                return;
            }

            _localAnt = new LocalAnt(_antNames[_localAntModel.GetName()], _localAntModel);

            StartAnt();
        }

        private void StartAnt()
        {
            int id = _localAnt.GetAnts().GetId();
            var host = new JArray();

            foreach (var address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                host.Add(address.GetAddressBytes());

            var args = new JObject();
            args.Add("id", id);
            args.Add("port", AntPipeline.GetAntPipeline().GetPort());
            args.Add("host", host);
            _queenChannel.SendMessage("request_join_ant", args);
        }

        public void ApproveJoinAnt(int id, IEnumerable<JObject> network)
        {
            _localAnt.SetId(id);
            _ants[_localAnt.GetAnts().GetId()].JoinAnt(_localAnt);
            foreach (var ant_packet in network)
            {
                var ants = _ants[ant_packet.Value<int>("ant")];
                var ant = new Ant(ants, ant_packet.Value<int>("id"));
                ants.JoinAnt(ant);

                int port = ant_packet.Value<int>("port");
                foreach (var host in ant_packet.Value<JArray>("host").Values<byte[]>())
                {
                    AntPipeline.GetAntPipeline().AddPipeline(ant, new IPEndPoint(new IPAddress(host), port));
                }
                //이제 실질적으로 소켓 연결
            }
            _queenChannel.SendMessage("start_ant", null);
            _localAntModel.OnStart();
        }

        public IAnt GetLocalAnt()
        {
            return _localAnt;
        }

        public void HelloAnt(IChannel channel, int antsId, int id)
        {
            var ants = _ants[antsId];
            var ant = new Ant(ants, id);
            channel.SetTag(ant);
            ants.JoinAnt(ant);
        }
    }
}
