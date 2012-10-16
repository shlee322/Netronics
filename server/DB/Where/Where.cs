using System.Collections.Generic;

namespace Netronics.DB.Where
{
    public class Where
    {
        private List<List<Where>> _orData = new List<List<Where>>();
        private List<Where> _andData = new List<Where>();

        public Where Or(Where where)
        {
            if(_andData.Count!=0)
            {
                _orData.Add(_andData);
                _andData = new List<Where>();
            }
            return this;
        }

        public Where Match(Model model)
        {
            _andData.Add(new ModelWhere(model));
            return this;
        }

        public Where Match(string name, object value)
        {
            return this;
        }

        public Where String(string sql)
        {
            _andData.Add(new StringWhere(sql));
            return this;
        }
    }
}
