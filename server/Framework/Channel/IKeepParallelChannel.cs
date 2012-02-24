namespace Netronics.Channel
{
    interface IKeepParallelChannel : IChannel
    {
        bool SetParallel(bool parallel);
    }
}
