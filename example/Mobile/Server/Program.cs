using System;
using System.Security.Cryptography.X509Certificates;
using Netronics.DB.DBMS;
using Netronics.DB.Where;
using Netronics.Mobile;
using Netronics.DB;

namespace MobileServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Sqlite.UsingSqlite("test.db");

            var mobile = new Mobile(7777, new X509Certificate2("server.pfx", "asdf"));
            mobile.UseAuth = true;
            mobile.Connected = client =>
                {
                };
            mobile.Disconnected = client =>
                {
                };
            mobile.On("hi", request => { });
            mobile.On("hi", request => { }, 5);
            mobile.Run();/*
            var doc = new DB.Doc() {Id = 1, Title = "테스트", Content = "테스트입니다.", Date = DateTime.Now};
            doc.Save();

            var test = Model.ModelObjects<DB.Doc>().Count;
            var docs = Model.ModelObjects<DB.Doc>().Find(new Where().Match(new DB.Doc(){Title = "테스트"}));
            */
        }
    }
}
