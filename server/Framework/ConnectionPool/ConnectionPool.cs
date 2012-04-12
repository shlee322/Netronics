using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Netronics.DB
{
    class ConnectionPool : IDisposable
    {
        private static Dictionary<string, Func<object>> ConnectionFuncDict = new Dictionary<string, Func<object>>();
        private static Dictionary<string, ConcurrentQueue<object>> Pool = new Dictionary<string, ConcurrentQueue<object>>();

        protected bool disposed = false;
        protected string _name;
        protected object _conn;

        public static void AddConnection(string name, Func<object> ConnectionFunc)
        {
            ConnectionFuncDict.Add(name, ConnectionFunc);
        }

        public static void EnqueueConnection(string name, object conn)
        {
            Pool[name].Enqueue(conn);
        }

        public static object DequeueConnection(string name)
        {
            object conn;

            if (Pool[name].TryDequeue(out conn))
            {
                return conn;
            }

            return ConnectionFuncDict[name];
        }

        public ConnectionPool(string name)
        {
            _name = name;
            _conn = DequeueConnection(name);
        }

        ~ConnectionPool()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }

                if (_conn != null)
                {
                    EnqueueConnection(_name, _conn);
                    _conn = null;
                }

                disposed = true;
            }
        }
    }
}
