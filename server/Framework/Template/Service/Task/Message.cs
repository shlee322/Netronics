using System;

namespace Netronics.Template.Service.Task
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Message : Attribute
    {
        private readonly string _targetService;
        private readonly uint _take;

        public Message(string targetService, uint take=1)
        {
            _targetService = targetService;
            _take = take;
        }

        public string GetTargetService()
        {
            return _targetService;
        }

        public uint GetTake()
        {
            return _take;
        }
    }
}
