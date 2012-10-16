package kr.lerad.netronics.mobile.android;

import org.jboss.netty.buffer.ChannelBuffer;
import org.jboss.netty.buffer.ChannelBuffers;
import org.jboss.netty.channel.Channel;
import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.handler.codec.frame.FrameDecoder;
import org.jboss.netty.handler.codec.oneone.OneToOneEncoder;
import org.msgpack.MessagePack;
import org.msgpack.type.Value;

import java.io.ByteArrayOutputStream;
import java.util.Map;

public class PacketEncoder extends OneToOneEncoder {
    private static final MessagePack messagePack = new MessagePack();

    protected Object encode(
            ChannelHandlerContext ctx, Channel channel, Object msg) throws Exception {
        if (!(msg instanceof Value)) {
            return null;
        }

        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        messagePack.write(stream, msg);

        int dataLength = stream.size();

        ChannelBuffer buf = ChannelBuffers.dynamicBuffer();

        buf.writeByte(1);
        buf.writeInt(dataLength);
        buf.writeBytes(stream.toByteArray());

        return buf;
    }


}
