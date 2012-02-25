namespace Netronics.Channel.Channel
{
    interface IKeepParallelChannel : IChannel
    {
        bool SetParallel(bool parallel);
    }
}
