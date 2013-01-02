using System.Threading;
using Netronics.Channel.Channel;
using Netronics.Scheduling;
using Newtonsoft.Json.Linq;
using Netronics.Ant.Packet;

namespace Netronics.Ant.QueenAnt
{
    class AntManager
    {
        private static readonly AntManager Instance = new AntManager();

        private Scheduling.Scheduler _scheduler = new Scheduler(1);
        private readonly AutoResetEvent _workEvent = new AutoResetEvent(false);

        public static AntManager GetAntManager()
        {
            return Instance;
        }

        public void RequestJoinAnt(IChannel channel, JObject args)
        {
            _scheduler.QueueWorkItem(0, () => RequestJoinAnt_1(channel, args));
        }

        private void RequestJoinAnt_1(IChannel channel, JObject args)
        {
            var ants = QueenAnt.GetQueenAnt().GetAnts(args.Value<int>("id"));
            var ant = ants.JoinAnt(channel, args);

            var network = QueenAnt.GetQueenAnt().GetNetwork(ant);

            var packet = new JObject();
            packet.Add("id", ant.GetId());
            packet.Add("network", new JArray(network));
            channel.SendMessage("approve_join_ant", packet);
            _workEvent.WaitOne();
        }

        public void StartAnt(Ant ant)
        {
            _workEvent.Set();
        }
    }
}
