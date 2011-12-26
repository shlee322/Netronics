using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netronics;

namespace Test
{
    [TestClass]
    public class BSONEnDecoderTest
    {
        [TestMethod]
        public void BSONEnDecoderTest1()
        {
            dynamic data = new Newtonsoft.Json.Linq.JObject();
            data.test1 = "test";
            data.test2 = 123;
            data.test3 = true;
            data.test4 = new Newtonsoft.Json.Linq.JObject();
            data.test4.a = "abcd";

            BSONEncoder encoder = new BSONEncoder();
            PacketBuffer buffer = encoder.encode(data);

            BSONDecoder decoder = new BSONDecoder();
            dynamic data2 = decoder.decode(buffer);

            System.Console.WriteLine(data2);
        }
    }
}
