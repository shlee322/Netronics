using Netronics.Ant.Ant;
using Netronics.Ant.QueenAnt;

namespace Netronics.Ant
{
    class Loader
    {
        private AntConfig _config;

        public static void Load(AntConfig config)
        {
            Loader loader = null;
            switch (config.GetServerType())
            {
                case "queen":
                    loader = new QueenLoader();
                    break;
                case "ant":
                    loader = new AntLoader();
                    break;
            }
            if (loader == null)
                return;
            loader._config = config;
            loader.Load();
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
