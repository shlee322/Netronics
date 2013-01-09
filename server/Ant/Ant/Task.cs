using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant
{
    public class Task
    {
        private IAnt _ant;
        private int _tId;
        private int _type;
        private JToken _message;

        public Task(IAnt ant, int tid, int type, JToken msg)
        {
            _ant = ant;
            _tId = tid;
            _type = type;
            _message = msg;
        }

        public IAnt GetAnt()
        {
            return _ant;
        }

        public int GetMessageType()
        {
            return _type;
        }

        public JToken GetMessage()
        {
            return _message;
        }

        public void Response(JToken message)
        {
            _ant.SendResponseTask(_tId, message);
        }
    }
}
