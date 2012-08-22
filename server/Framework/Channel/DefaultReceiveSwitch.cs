namespace Netronics.Channel
{
    class DefaultReceiveSwitch : IReceiveSwitch
    {
        public int ReceiveSwitching(IReceiveContext context)
        {
            return context.GetChannel().GetHashCode();
        }
    }
}
