namespace Netronics.Channel
{
    class DefaultReceiveSwitch : IReceiveSwitch
    {
        public static DefaultReceiveSwitch Switch = new DefaultReceiveSwitch();
        public int ReceiveSwitching(IReceiveContext context)
        {
            return context.GetChannel().GetHashCode();
        }
    }
}
