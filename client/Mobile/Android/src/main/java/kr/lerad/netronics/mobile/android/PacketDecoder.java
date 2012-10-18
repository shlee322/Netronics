package kr.lerad.netronics.mobile.android;

import com.fasterxml.jackson.databind.ObjectMapper;
import de.undercouch.bson4jackson.BsonFactory;
import org.jboss.netty.buffer.ChannelBuffer;
import org.jboss.netty.channel.Channel;
import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.handler.codec.frame.FrameDecoder;

import java.util.Map;

class PacketDecoder extends FrameDecoder {
    private static final ObjectMapper objectMapper = new ObjectMapper(new BsonFactory());

    protected Object decode(
            ChannelHandlerContext ctx, Channel channel, ChannelBuffer buffer) throws Exception {
        if (buffer.readableBytes() < 5) {
            return null;
        }

        buffer.markReaderIndex();

        buffer.readByte();
        int dataLength = buffer.readInt();
        if (buffer.readableBytes() < dataLength) {
            buffer.resetReaderIndex();
            return null;
        }

        byte[] decoded = new byte[dataLength];
        buffer.readBytes(decoded);

        return objectMapper.readValue(decoded, 0, dataLength, Map.class);
    }
}
