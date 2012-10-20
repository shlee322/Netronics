using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using Netronics.DB.Where;

namespace Netronics.DB.DBMS
{
    public class Sqlite : DB
    {
        private readonly SQLiteConnectionStringBuilder _connectionStringBuilder = new SQLiteConnectionStringBuilder();

        public static void UsingSqlite(string file)
        {
            Instance = new Sqlite(file);
        }

        private Sqlite(string file)
        {
            _connectionStringBuilder.DataSource = file;
        }

        public override void CreateTable(string tableName, IEnumerable<FieldData> fieldInfos)
        {
            var writer = new StringWriter();
            writer.Write("CREATE TABLE `{0}` (`id` INTEGER PRIMARY KEY AUTOINCREMENT", tableName);
            foreach (var field in fieldInfos)
            {
                if (field.GetField().GetType() == typeof (CharField))
                {
                    writer.Write(", `{0}` TEXT NOT NULL", field.GetInfo().Name.ToLower());
                }
                else if (field.GetField().GetType() == typeof(Int64Field))
                {
                    writer.Write(", `{0}` INTEGER", field.GetInfo().Name.ToLower());
                }
                else if (field.GetField().GetType() == typeof(DateTimeField))
                {
                    //updated TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
                    writer.Write(", `{0}` INTEGER", field.GetInfo().Name.ToLower());
                }
            }
            writer.Write(");");
            var conn = new SQLiteConnection(_connectionStringBuilder.ToString());
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = writer.ToString();

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public override long Save(string tableName, IEnumerable<FieldData> dbField, Model model)
        {
            var writer = new StringWriter();

            writer.Write("INSERT OR REPLACE INTO `{0}`(`id`", tableName);

            foreach (var fieldData in dbField)
                writer.Write(", `{0}`", fieldData.GetInfo().Name.ToLower());

            /*
            writer.Write(") values ({0}", model.Id == -1 ? "null" : model.Id.ToString());

            foreach (var fieldData in dbField)
                writer.Write(", '{0}'", fieldData.GetInfo().GetValue(model));
            */
            writer.Write(") values ({0}", model.Id == -1 ? "null" : model.Id.ToString());

            foreach (var fieldData in dbField)
                writer.Write(", '{0}'", fieldData.GetInfo().GetValue(model));

            writer.Write(");");

            var conn = new SQLiteConnection(_connectionStringBuilder.ToString());
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = writer.ToString();
            /*
            cmd.Parameters.AddWithValue("@id", model.Id == -1 ? "null" : model.Id.ToString());
            foreach (var fieldData in dbField)
                cmd.Parameters.AddWithValue("@" + fieldData.GetInfo().Name.ToLower(), fieldData.GetInfo().GetValue(model));
            */
            cmd.ExecuteNonQuery();
            long id = conn.LastInsertRowId;
            conn.Close();

            return id;
        }

        public override NameValueCollection[] Find(string tableName, Where.Where where)
        {
            var collections = new List<NameValueCollection>();

            var conn = new SQLiteConnection(_connectionStringBuilder.ToString());
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM `" + tableName + "` WHERE " + GetWhereSQL(where);


            
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                collections.Add(reader.GetValues());
            }
            reader.Close();
            conn.Close();

            return collections.ToArray();
        }

        private string GetWhereSQL(Where.Where where)
        {
            var writer = new StringWriter();
            where.Or(null);
            foreach (var ordata in where.GetOrData())
            {
                writer.Write("(");
                foreach (var anddata in ordata)
                {
                    if(anddata is ModelWhere)
                    {
                        writer.Write("(");
                        foreach (var field in ((ModelWhere)anddata).GetModel().GetFieldData())
                        {
                            var value = field.GetInfo().GetValue(((ModelWhere) anddata).GetModel());

                            if (value == null)
                                continue;
                            if(value is long && ((long)value) == 0)
                                continue;
                            
                            writer.Write("{0} = '{1}' and ", field.GetInfo().Name.ToLower(), value); 
                        }
                        writer.Write("true)");
                    }
                    else if (anddata is StringWhere)
                    {
                        
                    }
                    else
                    {
                        writer.Write(GetWhereSQL(anddata));
                    }
                    writer.Write(" and ");
                }

                writer.Write("true) or ");
            }
            writer.Write("false");

            return writer.ToString().Replace(" or false","").Replace(" and true","");
        }

        public override NameValueCollection Find(string tableName, int id)
        {
            NameValueCollection collection = null;

            var conn = new SQLiteConnection(_connectionStringBuilder.ToString());
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM `" + tableName + "` WHERE `id`=@id ";
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                collection = reader.GetValues();
            }
            reader.Close();
            conn.Close();
            return collection;
        }
    }
}
