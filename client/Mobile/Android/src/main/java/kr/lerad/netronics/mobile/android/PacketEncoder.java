package kr.lerad.netronics.mobile.android;

import com.fasterxml.jackson.databind.ObjectMapper;
import de.undercouch.bson4jackson.BsonFactory;
import org.jboss.netty.buffer.ChannelBuffer;
import org.jboss.netty.buffer.ChannelBuffers;
import org.jboss.netty.channel.Channel;
import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.handler.codec.frame.FrameDecoder;
import org.jboss.netty.handler.codec.oneone.OneToOneEncoder;

import java.io.ByteArrayOutputStream;
import java.util.Map;

class PacketEncoder extends OneToOneEncoder {
    private static final ObjectMapper objectMapper = new ObjectMapper(new BsonFactory());

    protected Object encode(
            ChannelHandlerContext ctx, Channel channel, Object msg) throws Exception {
        if (!(msg instanceof Map)) {
            return msg;
        }

        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        objectMapper.writeValue(stream, msg);

        int dataLength = stream.size();

        ChannelBuffer buf = ChannelBuffers.dynamicBuffer();

        buf.writeByte(1);
        buf.writeInt(dataLength);
        buf.writeBytes(stream.toByteArray());

        return buf;
    }


}
