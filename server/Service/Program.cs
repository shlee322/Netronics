using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Service.Service.Loader;
using log4net.Config;

namespace Service
{
    class Program
    {
        public static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        static void ServiceLoad(string servicesFilePath)
        {
            if (!File.Exists(servicesFilePath))
                throw new Exception("system.ns 파일이 존재하지 않습니다.");

            var servicesDirectory = new FileInfo(servicesFilePath).DirectoryName;
            var serviceList = new List<string>();
            foreach (var line in File.ReadLines(servicesFilePath))
            {
                if (serviceList.Exists((str) => str == line.ToLower()))
                    throw new Exception("system.ns에 중복된 서비스 이름이 존재합니다.");

                serviceList.Add(line.ToLower());
                if (line.ToLower() == "servicemanager")
                {
                    Manager.ServiceManager.Load(servicesDirectory);
                    continue;
                }

                if (!File.Exists(servicesDirectory + "/" + line))
                    throw new Exception(line + " 서비스를 찾을 수 없습니다.");


                ServiceLoader.Load(servicesDirectory, line);
            }
        }

        static void Main(string[] args)
        {
            //차후 명령처리를 위해서...

            var servicesFilePath = AppDomain.CurrentDomain.BaseDirectory + "system.ns";
            if (args.Length > 0)
                servicesFilePath = args[args.Length - 1];

            Directory.SetCurrentDirectory(new FileInfo(servicesFilePath).DirectoryName);
            DOMConfigurator.Configure(new FileInfo("log4net.xml"));

            //테스트
            servicesFilePath = @"C:\Users\Sanghyuck\Projects\Parfe Server\bin\system.ns";

            ServiceLoad(servicesFilePath);

            ExitEvent.WaitOne();
        }
    }
}
