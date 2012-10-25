using Netronics.Channel;

namespace SslServer
{
    class Handler : IChannelHandler
    {
        // 클라이언트가 접속시 호출
        public void Connected(IReceiveContext channel)
        {
        }

        // 클라이언트 접속종료시 호출
        public void Disconnected(IReceiveContext channel)
        {
        }

        // 클라이언트로부터 메시지가 왔을때 호출
        public void MessageReceive(IReceiveContext context)
        {
            context.GetChannel().SendMessage(context.GetMessage()); // 에코 서버인 만큼 온 데이터를 그대로 전송
        }
    }
}
