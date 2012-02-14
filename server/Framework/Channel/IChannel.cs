namespace Netronics.Channel
{
    public interface IChannel
    {
        void Connect();
        void Disconnect();
        void SendMessage(dynamic message);
    }
}