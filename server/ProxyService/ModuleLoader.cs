using System;
using System.IO;
using System.Reflection;

namespace ProxyService
{
    class ModuleLoader
    {
        public static void LoadModules()
        {
            if(!Directory.Exists("./Modules/"))
                throw new Exception("Modules Directory가 존재하지 않습니다.");
            foreach (var moduleDir in Directory.GetDirectories("./Modules/"))
                new ModuleLoader(moduleDir);
        }

        private ModuleLoader(string moduleDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(moduleDir);
            Assembly controller = Assembly.LoadFile(String.Format("{0}/{1}.dll",dirInfo.FullName, dirInfo.Name));
            controller.GetType(String.Format("{0}.{0}", dirInfo.Name)).GetMethod("Load").Invoke(null, null);
        }
    }
}
