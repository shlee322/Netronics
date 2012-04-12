using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Framework
{
    [TestFixture()]
    class MySqlTest
    {
        [Test()]
        public void MySqlTest1()
        {
            Netronics.ConnectionPool.MySql.AddConnection("test", "127.0.0.1", "test", "test", "test");
            Netronics.ConnectionPool.MySql.ExecuteNonQuery("test", string.Format("INSERT INTO `test` (`v1`, `v2`, `v3`) VALUES ('{0}', '{1}', '{2}');", "test1", "test2", "test3"));

            using (var mysql = new Netronics.ConnectionPool.MySql("test"))
            {
                var reader = mysql.ExecuteReader("SELECT * FROM test;");
                while (reader.Read())
                {
                    Console.WriteLine(reader["v2"]);
                }
            }
        }
    }
}
