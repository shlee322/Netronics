using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace Netronics.DB.DBMS
{
    class MySQL : DB
    {
        private readonly SQLiteConnectionStringBuilder _connectionStringBuilder = new SQLiteConnectionStringBuilder();

        public static void UsingMySQL(string file)
        {
            Instance = new MySQL(file);
        }

        private MySQL(string file)
        {
            _connectionStringBuilder.DataSource = file;
        }

        public override void CreateTable(string tableName, IEnumerable<FieldData> fieldInfos)
        {
            var writer = new StringWriter();
            writer.Write("CREATE TABLE `{0}` (`id` int(11) NOT NULL AUTO_INCREMENT", tableName);
            foreach (var field in fieldInfos)
            {
                if (field.GetField().GetType() == typeof (CharField))
                {
                    writer.Write(", `{0}` varchar(256) NOT NULL", field.GetInfo().Name.ToLower());
                }
                else if (field.GetField().GetType() == typeof(DateTimeField))
                {
                    //updated TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
                    writer.Write(", `{0}` TIMESTAMP DEFAULT CURRENT_TIMESTAMP", field.GetInfo().Name.ToLower());
                }
            }
            writer.Write(", PRIMARY KEY (`id`));");

            var conn = new SQLiteConnection(_connectionStringBuilder.ToString());
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = writer.ToString();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public override void Save(string tableName, IEnumerable<FieldData> dbField, Model model)
        {
            var writer = new StringWriter();

            writer.Write("INSERT INTO `{0}`(`id`", tableName);

            foreach (var fieldData in dbField)
                writer.Write(", `{0}`", fieldData.GetInfo().Name.ToLower());
            writer.Write(") values ({0}", model.Id == -1 ? "null" : model.Id.ToString());

            foreach (var fieldData in dbField)
                writer.Write(", '{0}'", fieldData.GetInfo().GetValue(this));

            writer.Write(")");

            if (model.Id != -1)
            {
                writer.Write(" ON DUPLICATE KEY UPDATE `id`={0}", model.Id);
                foreach (var fieldData in dbField)
                    writer.Write(", `{0}`='{1}'", fieldData.GetInfo().Name.ToLower(), fieldData.GetInfo().GetValue(this));
                writer.Write(";");
            }

            var conn = new SQLiteConnection(_connectionStringBuilder.ToString());
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = writer.ToString();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
