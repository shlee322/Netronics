using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace Netronics.DB.DBMS
{
    public class DB
    {
        protected static DB Instance = new DB();

        public static DB GetInstance()
        {
            return Instance;
        }

        protected DB()
        {
        }

        public virtual void CreateTable(string tableName, IEnumerable<FieldData> fieldInfos)
        {
            throw new Exception("DB 설정 에러");
        }

        public virtual long Save(string tableName, IEnumerable<FieldData> dbField, Model model)
        {
            throw new Exception("DB 설정 에러");
        }

        public virtual long GetCount(string tableName)
        {
            throw new Exception("DB 설정 에러");
        }

        public virtual NameValueCollection[] Find(string tableName, Where.Where where)
        {
            throw new Exception("DB 설정 에러");
        }

        public virtual NameValueCollection Find(string tableName, int id)
        {
            throw new Exception("DB 설정 에러");
        }
    }
}
