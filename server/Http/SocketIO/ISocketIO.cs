using System;
using Newtonsoft.Json.Linq;

namespace Netronics.Http.SocketIO
{
    public interface ISocketIO
    {
        void On(string id, Action<dynamic> action);
        void Emit(string id, dynamic o);
    }
}
