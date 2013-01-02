using Netronics.Ant.Ant;
using Netronics.Ant.QueenAnt;
using log4net.Config;

namespace Netronics.Ant
{
    class Loader
    {
        private AntConfig _config;

        public static void Load(AntConfig config)
        {
            var logFile = config.GetData("log") != null ? config.GetData("log").ToObject<string>() : @".\log.xml";
            XmlConfigurator.Configure(new System.IO.FileInfo(logFile));

            switch (config.GetServerType())
            {
                case "queen":
                    QueenAnt.QueenAnt.Init(config);
                    break;
                case "ant":
                    Kernel.Init(config);
                    break;
            }
        }

        protected AntConfig GetConfig()
        {
            return _config;
        }

        protected virtual void Load()
        {
        }
    }
}
