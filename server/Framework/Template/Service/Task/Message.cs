using System;

namespace Netronics.Template.Service.Task
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Message : Attribute
    {
        public Message(string targetService, uint take=1)
        {
        }
    }
}
