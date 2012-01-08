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
                transaction.CreateTransaction(new Job("test" + i));

            System.Console.WriteLine(transaction.GetTransaction("0").GetServiceName());

            System.Console.WriteLine(transaction.GetTransaction("3").GetServiceName());

            System.Console.WriteLine(transaction.GetTransaction("40").GetServiceName());

            System.Console.WriteLine(transaction.GetTransaction("40"));

            System.Console.WriteLine(transaction.GetTransaction("60"));

            transaction.Dispose();

            System.Console.WriteLine(transaction.GetTransaction("60"));
        }
    }
}
