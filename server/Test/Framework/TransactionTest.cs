using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Netronics;

namespace Test
{
    [TestFixture()]
    public class TransactionTest
    {
        [Test()]
        public void TransactionTest1()
        {
            Transaction transaction = new Transaction();
            for(int i=0; i<50; i++)
                transaction.createTransaction(new Job("test" + i));

            System.Console.WriteLine(transaction.getTransaction("0").getServiceName());

            System.Console.WriteLine(transaction.getTransaction("3").getServiceName());

            System.Console.WriteLine(transaction.getTransaction("40").getServiceName());

            System.Console.WriteLine(transaction.getTransaction("40"));

            System.Console.WriteLine(transaction.getTransaction("60"));
        }
    }
}
