namespace Netronics.Channel
{
    public interface IReceiveSwitch
    {
        int ReceiveSwitching(IReceiveContext context);
    }
}
