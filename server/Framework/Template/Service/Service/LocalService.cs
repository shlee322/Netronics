using System;
using System.Collections.Generic;
using Netronics.Template.Service.Task;

namespace Netronics.Template.Service.Service
{
    class LocalService : Service
    {
        private readonly Dictionary<Type, Action<Task.Task>> _processorList = new Dictionary<Type, Action<Task.Task>>();
 
        public void AddProcessor(Type type, Action<Task.Task> processor)
        {
            GetProcessorMessage(type);
            _processorList.Add(type, processor);
        }

        private Message GetProcessorMessage(Type type)
        {
            object[] o = type.GetCustomAttributes(typeof(Message), true);
            if (o.Length == 0)
                throw new Exception("Message");
            return (Message)o[o.Length - 1];
        }
    }
}
