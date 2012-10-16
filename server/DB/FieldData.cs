using System.Reflection;

namespace Netronics.DB
{
    public class FieldData
    {
        private readonly FieldInfo _info;
        private readonly Field _field;

        public FieldData(FieldInfo info, Field field)
        {
            _info = info;
            _field = field;
        }

        public FieldInfo GetInfo()
        {
            return _info;
        }

        public Field GetField()
        {
            return _field;
        }
    }
}
