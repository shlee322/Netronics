using System.IO;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant
{
    class AntConfig
    {
        private JToken _data;

        public static AntConfig Load(string path)
        {
            var fileInfo = new FileInfo(path);
            if(!fileInfo.Exists)
            {
                System.Console.WriteLine("Config File Error " + path);
                return null;
            }

            Directory.SetCurrentDirectory(fileInfo.DirectoryName);

            return new AntConfig(path);
        }

        private AntConfig(string path)
        {
            _data = JToken.Parse(File.ReadAllText(path));
        }

        public string GetServerType()
        {
            var type = _data["type"];
            return type == null ? "ant" : type.ToObject<string>();
        }

        public JToken GetData(object key)
        {
            return _data[key];
        }
    }
}
