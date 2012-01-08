using System;
using System.Collections.Generic;

namespace Netronics
{
    public class Transaction
    {
        private LinkedList<Item> list = new LinkedList<Item>();
        private long nextTransactionID;

        public string createTransaction(Job job)
        {
            var item = new Item(Convert.ToString(nextTransactionID++), job);
            lock (this)
            {
                list.AddLast(item);
            }
            return item.getTransactionID();
        }

        public Job getTransaction(string id)
        {
            Job job = null;
            lock (this)
            {
                if (list == null)
                    return null;

                LinkedListNode<Item> node = list.Find(new Item(id, null));
                if (node == null)
                    return null;
                job = node.Value.getJob();
                list.Remove(node);
            }
            return job;
        }

        public LinkedList<Item> Dispose()
        {
            LinkedList<Item> item = list;
            lock (this)
            {
                list = null;
                return item;
            }
        }

        #region Nested type: Item

        public class Item
        {
            private readonly string id;
            private readonly Job job;

            public Item(string id, Job job)
            {
                this.id = id;
                this.job = job;
            }

            public string getTransactionID()
            {
                return id;
            }

            public Job getJob()
            {
                return job;
            }

            public override int GetHashCode()
            {
                return getTransactionID().GetHashCode();
            }

            public override bool Equals(object i)
            {
                return GetHashCode() == (i).GetHashCode();
            }
        }

        #endregion
    }
}