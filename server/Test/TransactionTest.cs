using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netronics;

namespace Test
{
    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void TransactionTest1()
        {
            Transaction transaction = new Transaction();
            for(int i=0; i<50; i++)
                transaction.createTransaction(new Job("test" + i));

            System.Console.WriteLine(transaction.getTransaction("0").getSerivceName());

            System.Console.WriteLine(transaction.getTransaction("3").getSerivceName());

            System.Console.WriteLine(transaction.getTransaction("40").getSerivceName());

            System.Console.WriteLine(transaction.getTransaction("40"));

            System.Console.WriteLine(transaction.getTransaction("60"));
        }
    }
}
