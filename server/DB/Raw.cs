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


        public long Count
        {
            get
            {
                return DBMS.DB.GetInstance().GetCount(_tableName);
            }
        }

        public Model[] Find(Where.Where where)
        {
            var data = DBMS.DB.GetInstance().Find(_tableName, where);

            var list = new List<Model>();
            foreach (var row in data)
            {
                var obj = (Model)Activator.CreateInstance(_type);
                obj.Id = Convert.ToInt64(row.Get("id"));
                foreach (var fieldData in _dbField)
                {
                    if(fieldData.GetField() is CharField)
                        fieldData.GetInfo().SetValue(obj, row.Get(fieldData.GetInfo().Name.ToLower()));
                    else if(fieldData.GetField() is Int64Field)
                        fieldData.GetInfo().SetValue(obj, Convert.ToInt64(row.Get(fieldData.GetInfo().Name.ToLower())));
                }
                list.Add(obj);
            }

            return list.ToArray();
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
