using System;
using System.Collections.Generic;
using System.IO;
namespace Netronics.DB
{
    public class Model
    {
        public int Id=-1;
        private readonly List<FieldData> _dbField = new List<FieldData>();

        public Model()
        {
            foreach (var field in GetType().GetFields())
            {
                var attributes = field.GetCustomAttributes(typeof(Field), true);
                if (attributes.Length < 1)
                    continue;
                var attribute = attributes[0];
                _dbField.Add(new FieldData(field, attribute as Field));
            }
        }

        public void Save()
        {
            var tableName = GetType().FullName.Replace(".", "_").ToLower();

            DBMS.DB.GetInstance().Save(tableName, _dbField, this);
        }

        public static Raw ModelObjects<T>()
        {
            return Raw.GetModelObjects(typeof(T));
        }

        public Raw Objects
        {
            get { return Raw.GetModelObjects(GetType()); }
        }

        protected DateTime DateTimeField()
        {
            return DateTime.Now;
        }

        protected object ForeignKey(Type type)
        {
            return null;
        }
    }
}