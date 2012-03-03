using System;
using System.Collections.Generic;
using Netronics.Template.Service.Task;

namespace Netronics.Template.Service.Service
{
    public class LocalService : Service
    {
        public class Processor
        {
            public Type MessageType;
            public Action<Task.Task> Action;
        }
        private readonly byte[] _id;
        private readonly Dictionary<string, Processor> _processorList = new Dictionary<string, Processor>();
 
        public LocalService(byte[] id)
        {
            if(id.Length != 4)
                throw new Exception("ID 길이가 잘못되었습니다.");
            _id = id;
        }

        public byte[] GetID()
        {
            return _id;
        }
        
        public void AddProcessor(Type type, Action<Task.Task> processor)
        {
            GetProcessorMessage(type);
            var p = new Processor();
            p.MessageType = type;
            p.Action = processor;
            _processorList.Add(p.MessageType.FullName, p);
        }

        public Processor GetProcessor(string messageType)
        {
            return _processorList[messageType];
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
