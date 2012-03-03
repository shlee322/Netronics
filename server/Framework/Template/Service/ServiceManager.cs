using System;
using System.Collections.Generic;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Template.Service.Protocol;
using Netronics.Template.Service.Service;

namespace Netronics.Template.Service
{
    public class ServiceManager : IChannelHandler
    {
        private readonly LocalService _localService;
        private readonly Dictionary<uint, Service.Service> _services = new Dictionary<uint, Service.Service>(); 

        public ServiceManager(LocalService localService)
        {
            _localService = localService;
            _localService.SetServiceManager(this);
        }

        public void ProcessingTask(Task.Task task)
        {
        }

        public LocalService GetLocalService()
        {
            return _localService;
        }

        public Type GetMessageType(string messageName)
        {
            return _localService.GetProcessor(messageName).MessageType;
        }


        public void Connected(IChannel channel)
        {
            channel.SendMessage(new IDInfo { ID = _localService.GetID()});
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            if (message is Request)
                ProcessingRequest(message);
        }

        private void ProcessingRequest(Request request)
        {
            _localService.GetProcessor(request.Message.GetType().FullName).Action(Task.Task.GetTask(request));
        }
    }
}