using System;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Netronics.ConnectionPool
{
    public class MySql : ConnectionPool
    {
        private MySqlDataReader _reader;

        public static void AddConnection(string name, string server, string user_id, string password, string database, int port = 0)
        {
            AddConnection(name, () =>
                {
                    var connection = new MySqlConnection(string.Format("data source={0}; database={1}; user id={2}; password={3}", server, database, user_id, password));
                    connection.Open();
                    return connection;
                });
        }

        public static int ExecuteNonQuery(string name, string query)
        {
            using (var conn = new MySql(name))
            {
                return conn.ExecuteNonQuery(query);
            }
        }

        public MySql(string name) : base(name)
        {
            var mysql = _conn as MySqlConnection;
            if(mysql != null && mysql.State == ConnectionState.Closed)
                mysql.Open();
        }

        public int ExecuteNonQuery(string query)
        {
            using (var cmd = new MySqlCommand(query, (MySqlConnection)_conn))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public MySqlDataReader ExecuteReader(string query)
        {
            if (_reader != null)
                _reader.Dispose();
            using (var cmd = new MySqlCommand(query, (MySqlConnection)_conn))
            {
                _reader = cmd.ExecuteReader();
                return _reader;
            }
        }

        public MySqlCommand CreateCommand()
        {
            return ((MySqlConnection) _conn).CreateCommand();
        }
    }
}
