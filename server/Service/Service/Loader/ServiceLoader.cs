using System;
using System.IO;
using System.Reflection;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Service.Service.Manager;

namespace Service.Service.Loader
{
    class ServiceLoader
    {
        private IServiceInfo _info;
        private ManagerProcessor _manager;
        
        private LocalService _service;

        public static void Load(string path, string name)
        {
            new ServiceLoader(path, name);
        }

        private ServiceLoader(string path, string name)
        {
            Assembly asm = Assembly.LoadFrom(path + "/" + name);
            var info = asm.GetType(asm.FullName.Substring(0, asm.FullName.IndexOf(",")) + ".ServiceInfo");
            if(info == null || !typeof(IServiceInfo).IsAssignableFrom(info))
                throw new Exception(asm.FullName.Substring(0, asm.FullName.IndexOf(",")) + ".ServiceInfo가 존재하지 않거나 IServiceInfo를 상속받지 않습니다.");


            _info = (IServiceInfo)Activator.CreateInstance(info);
            _service = _info.GetService();

            _manager = new ManagerProcessor(this, File.ReadAllLines(path + "/manager.ns"));
        }

        public string GetServiceName()
        {
            return _info.GetServiceName();
        }

        public LocalService GetService()
        {
            return _service;
        }
    }
}
