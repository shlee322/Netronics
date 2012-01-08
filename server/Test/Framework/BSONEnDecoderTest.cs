using System;
using NUnit.Framework;
using Netronics;
using Newtonsoft.Json.Linq;

namespace Test
{
    [TestFixture]
    public class BSONEnDecoderTest
    {
        [Test]
        public void BSONEnDecoderTest1()
        {
            dynamic data = new JObject();
            data.test1 = "test";
            data.test2 = 123;
            data.test3 = true;
            data.test4 = new JObject();
            data.test4.a = "abcd";

            var encoder = new BsonEncoder();
            PacketBuffer buffer = encoder.Encode(data);

            var decoder = new BsonDecoder();
            dynamic data2 = decoder.Decode(buffer);
            Console.WriteLine(data2);
        }

        [Test]
        public void BSONEnDecoderTest_legibleBytes()
        {
            var buffer = new PacketBuffer();
            buffer.Write(100);
            var decoder = new BsonDecoder();
            if (decoder.Decode(buffer) != null)
                throw new Exception();
        }

        [Test]
        public void BSONEnDecoderTest_resetBufferIndex()
        {
            var buffer = new PacketBuffer();
            buffer.Write(100);
            buffer.Write(1);
            var decoder = new BsonDecoder();
            if (decoder.Decode(buffer) != null)
                throw new Exception();
        }
    }
}