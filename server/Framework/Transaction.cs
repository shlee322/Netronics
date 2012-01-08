using System;
using System.Collections.Generic;

namespace Netronics
{
    public class Transaction
    {
        private LinkedList<Item> _list = new LinkedList<Item>();
        private long _nextTransactionId;

        public string CreateTransaction(Job job)
        {
            var item = new Item(Convert.ToString(_nextTransactionId++), job);
            lock (this)
            {
                _list.AddLast(item);
            }
            return item.GetTransactionId();
        }

        public Job GetTransaction(string id)
        {
            Job job = null;
            lock (this)
            {
                if (_list == null)
                    return null;

                LinkedListNode<Item> node = _list.Find(new Item(id, null));
                if (node == null)
                    return null;
                job = node.Value.GetJob();
                _list.Remove(node);
            }
            return job;
        }

        public LinkedList<Item> Dispose()
        {
            LinkedList<Item> item = _list;
            lock (this)
            {
                _list = null;
                return item;
            }
        }

        #region Nested type: Item

        public class Item
        {
            private readonly string _id;
            private readonly Job _job;

            public Item(string id, Job job)
            {
                _id = id;
                _job = job;
            }

            public string GetTransactionId()
            {
                return _id;
            }

            public Job GetJob()
            {
                return _job;
            }

            public override int GetHashCode()
            {
                return GetTransactionId().GetHashCode();
            }

            public override bool Equals(object i)
            {
                return GetHashCode() == (i).GetHashCode();
            }
        }

        #endregion
    }
}