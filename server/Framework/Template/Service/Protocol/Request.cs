namespace Netronics.Template.Service.Protocol
{
    public class Request
    {
        public byte[] Sender;
        public byte[] Receiver;
        public ulong Transaction;
        public object Message;
    }
}
