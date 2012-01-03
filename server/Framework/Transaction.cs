using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    public class Transaction
    {
        private LinkedList<Item> list = new LinkedList<Item>();
        private long nextTransactionID = 0;


        class Item
        {
            private string id;
            private Job job;

            public Item(string id, Job job)
            {
                this.id = id;
                this.job = job;
            }

            public string getTransactionID()
            {
                return this.id;
            }

            public Job getJob()
            {
                return this.job;
            }

            public override int GetHashCode()
            {
                return this.getTransactionID().GetHashCode();
            }

            public override bool Equals(object i)
            {
                if (i.GetType() != typeof(Item))
                    return false;

                return this.getTransactionID() == ((Item)i).getTransactionID();
            }
        }

        public string createTransaction(Job job)
        {
            Item item = new Item(Convert.ToString(nextTransactionID++), job);
            lock (this.list)
            {
                this.list.AddLast(item);
            }
            return item.getTransactionID();
        }

        public Job getTransaction(string id)
        {
            Job job = null;
            lock (this.list)
            {
                LinkedListNode<Item> node = this.list.Find(new Item(id, null));
                if (node == null)
                    return null;
                job = node.Value.getJob();
                this.list.Remove(node);
            }
            return job;
        }
    }
}
