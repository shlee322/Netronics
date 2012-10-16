using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant
{
    class AntLoader : Loader
    {
        private string _fileName;
        private string _className;
        private Assembly _assembly;
        private AntModel _localAnt;
        private AntManager _antManager;

        private IPEndPoint[] _queenIPEndPoints;

        protected override void Load()
        {
            base.Load();

            LoadQueenIPEndPoint();

            LoadAssembly();

            _antManager = new AntManager(this);
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
            _localAnt = _assembly.CreateInstance(_className) as AntModel;
            if (_localAnt == null)
                throw new Exception("AntModel을 로드 할 수 없습니다.");
        }

        public IPEndPoint GetQueenIPEndPoint()
        {
            return _queenIPEndPoints[0];
        }
    }
}
