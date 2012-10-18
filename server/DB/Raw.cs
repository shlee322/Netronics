using System;
using System.Collections.Generic;

namespace Netronics.DB
{
    public class Raw
    {
        public static Raw GetModelObjects(Type type)
        {
            return new Raw(type);
        }

        private Type _type;
        private string _tableName;
        private readonly List<FieldData> _dbField = new List<FieldData>();

        private Raw(Type type)
        {
            _type = type;

            _tableName = _type.FullName.Replace(".", "_").ToLower();

            foreach (var field in type.GetFields())
            {
                var attributes = field.GetCustomAttributes(typeof(Field), true);
                if (attributes.Length < 1)
                    continue;
                var attribute = attributes[0];

                _dbField.Add(new FieldData(field, attribute as Field));
            }
        }


        public int Count
        {
            get
            {
                return DBMS.DB.GetInstance().GetCount(_tableName);
            }
        }

        public Model[] Find(Where.Where where)
        {
            return null;
        }

        public Model Find(int id)
        {
            var data = DBMS.DB.GetInstance().Find(_tableName, id);
            if (data == null)
                return null;
            var obj = (Model)Activator.CreateInstance(_type);
            obj.Id = Convert.ToInt64(data.Get("id"));
            foreach (var fieldData in _dbField)
            {
                fieldData.GetInfo().SetValue(obj, data.Get(fieldData.GetInfo().Name.ToLower()));
            }
            return obj;
        }
    }
}
