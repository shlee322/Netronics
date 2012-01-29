using Netronics.Channel;

namespace Netronics.Template.Service
{
    sealed class RemoteService : Service, IChannelHandler
    {
        private Channel.Channel _channel;

        public RemoteService(ServiceManager manager)
        {
            SetServiceManager(manager);
        }

        public void Connected(Channel.Channel channel)
        {
            _channel = channel;
        }

        public void Disconnected(Channel.Channel channel)
        {
        }

        public void MessageReceive(Channel.Channel channel, dynamic message)
        {
        }

        public override void ProcessingTask(Service service, Task task)
        {
            //해당 서비스로 task를 전송
        }
    }
}
