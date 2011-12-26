using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics
{
    public class BSONEncoder : PacketEncoder
    {
        protected JsonSerializer serializer = new JsonSerializer();
        public PacketBuffer encode(dynamic data)
        {
            PacketBuffer buffer = new PacketBuffer();

            MemoryStream stream = new MemoryStream();
            serializer.Serialize(new BsonWriter(stream), data);

            buffer.write((UInt32)stream.Length);
            stream.WriteTo(buffer.getBufferStream());

            return buffer;
        }
    }
}
