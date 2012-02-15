using System;
using NUnit.Framework;
using Netronics;
using Netronics.Protocol.PacketEncoder.Bson;
using Newtonsoft.Json.Linq;

namespace Framework
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
            PacketBuffer buffer = encoder.Encode(null, data);

            var decoder = new BsonDecoder();
            dynamic data2 = decoder.Decode(null, buffer);
            Console.WriteLine(data2);
        }

        [Test]
        public void BSONEnDecoderTestLegibleBytes()
        {
            var buffer = new PacketBuffer();
            buffer.Write(100);
            var decoder = new BsonDecoder();
            if (decoder.Decode(null, buffer) != null)
                throw new Exception();
        }

        [Test]
        public void BSONEnDecoderTestResetBufferIndex()
        {
            var buffer = new PacketBuffer();
            buffer.Write(100);
            buffer.Write(1);
            var decoder = new BsonDecoder();
            if (decoder.Decode(null, buffer) != null)
                throw new Exception();
        }
    }
}