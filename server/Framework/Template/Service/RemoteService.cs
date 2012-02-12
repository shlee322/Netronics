using Netronics.Channel;

namespace Netronics.Template.Service
{
    internal sealed class RemoteService : Service, IChannelHandler
    {
        private Channel.Channel _channel;

        public RemoteService(ServiceManager manager)
        {
            SetServiceManager(manager);
        }

        #region IChannelHandler Members

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

        #endregion

        public override void ProcessingTask(Task task)
        {
            //해당 서비스로 task를 전송
        }
    }
}