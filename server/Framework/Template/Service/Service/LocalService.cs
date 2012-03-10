using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Netronics.Template.Service.Message;
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
        private readonly int _id;
        private readonly Dictionary<string, Processor> _processorList = new Dictionary<string, Processor>();
        private readonly Dictionary<string, Type> _resultObjectList = new Dictionary<string, Type>();
        private readonly string _type;

        public LocalService(int id, string type)
        {
            _id = id;
            _type = type;

            AddProcessor(typeof(GetInfoMessage), GetInfo);
            AddResultObject(typeof(GetInfoResult));
        }

        public override void Start()
        {
        }

        public override int GetID()
        {
            return _id;
        }

        public override void ProcessingTask(Task.Task task)
        {
            GetProcessor(task.GetMessage().GetType().FullName).Action(task);
        }

        public override string GetServiceType()
        {
            return _type;
        }

        public override bool IsRunning()
        {
            return true;
        }

        public void AddProcessor(Type type, Action<Task.Task> processor)
        {
            GetProcessorMessage(type);
            var p = new Processor();
            p.MessageType = type;
            p.Action = processor;
            _processorList.Add(p.MessageType.FullName, p);
        }

        public void AddResultObject(Type type)
        {
            _resultObjectList.Add(type.FullName, type);
        }

        public Processor GetProcessor(string messageType)
        {
            return _processorList[messageType];
        }

        public Type GetResultObjectType(string typeName)
        {
            return _resultObjectList[typeName];
        }

        public Task.Message GetProcessorMessage(Type type)
        {
            object[] o = type.GetCustomAttributes(typeof(Task.Message), true);
            if (o.Length == 0)
                throw new Exception("Message");
            return (Task.Message)o[o.Length - 1];
        }

        private void GetInfo(Task.Task task)
        {
            task.Result(new GetInfoResult {Type = _type});
        }
    }
}
