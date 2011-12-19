using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics
{
    class BSONEncoder : PacketEncoder
    {
        protected JsonSerializer serializer = new JsonSerializer();
        public PacketBuffer encode(dynamic data)
        {
            PacketBuffer buffer = new PacketBuffer();
            serializer.Serialize(new BsonWriter(buffer.getBufferStream()), data);
            return buffer;
        }
    }
}
