package kr.lerad.netronics.mobile.android;

import org.jboss.netty.buffer.ChannelBuffer;
import org.jboss.netty.channel.Channel;
import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.handler.codec.frame.FrameDecoder;
import org.msgpack.MessagePack;

import java.util.Map;

public class PacketDecoder extends FrameDecoder {
    private static final MessagePack messagePack = new MessagePack();

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

        return messagePack.read(decoded);
    }
}
