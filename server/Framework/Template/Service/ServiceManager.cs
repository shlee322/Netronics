using System;
using System.Collections.Generic;
using Netronics.Channel;
using Netronics.Channel.Channel;
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

        public void Connected(IChannel channel)
        {
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            if (message is AssignServiceID)
            {
                AssignServiceID((AssignServiceID)message);
                return;
            }
        }
    }
}