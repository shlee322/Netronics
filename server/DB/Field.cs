using System;

namespace Netronics.DB
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Field : Attribute
    {
        protected Field()
        {
        }
    }
}
