using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Netronics.DB;
using Netronics.DB.DBMS;
using Newtonsoft.Json.Linq;

namespace DBTool
{
    class Program
    {
        /*
         * {
         *  model:[
         *   {
         *    assembly:"MobileServer.exe", list:["MobileServer.DB.Doc", "MobileServer.DB.Comment"]
         *   }
         *  ],
         *  
         * }
         */
        private static JToken _data;
        static void Main(string[] args)
        {
            Sqlite.UsingSqlite("test.db");
            if(args.Length < 1)
                throw new Exception("설정 파일을 로드해야 합니다.");
            Load(args[0]);
            foreach (var model in _data.Value<JArray>("model"))
            {
                var assembly = Assembly.LoadFrom(model.Value<string>("assembly"));
                foreach (var item in model.Value<JArray>("list"))
                {
                    var obj = assembly.GetType(item.ToObject<string>());
                    var tableName = item.ToObject<string>().Replace(".", "_").ToLower();
                    if(!obj.IsSubclassOf(typeof(Model)))
                    {
                        Console.WriteLine(obj + " Model을 상속해야 합니다.");
                        continue;
                    }

                    var fieldInfos = new List<FieldData>();

                    foreach (var field in obj.GetFields())
                    {
                        var attributes = field.GetCustomAttributes(typeof(Field), true);
                        if(attributes.Length < 1)
                            continue;
                        fieldInfos.Add(new FieldData(field, attributes[0] as Field));
                    }

                    DB.GetInstance().CreateTable(tableName, fieldInfos);
                }
            }
            
        }

        static void Load(string path)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                Console.WriteLine("Config File Error " + path);
                return;
            }
            Directory.SetCurrentDirectory(fileInfo.DirectoryName);
            _data = JToken.Parse(File.ReadAllText(path));
        }
    }
}
