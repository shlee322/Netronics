package kr.lerad.netronics.mobile.android;

import org.jboss.netty.channel.*;
import org.jboss.netty.handler.ssl.SslHandler;

import java.util.HashMap;
import java.util.logging.Level;
import java.util.logging.Logger;

public class Handler extends SimpleChannelUpstreamHandler {
    private static final Logger logger = Logger.getLogger(
            Handler.class.getName());

    private Mobile mobile;

    public Handler(Mobile mobile)
    {
        this.mobile = mobile;
    }

    @Override
    public void handleUpstream(
            ChannelHandlerContext ctx, ChannelEvent e) throws Exception {
        if (e instanceof ChannelStateEvent) {
            logger.info(e.toString());
        }
        super.handleUpstream(ctx, e);
    }

    @Override
    public void channelConnected(
            ChannelHandlerContext ctx, ChannelStateEvent e) throws Exception {
        SslHandler sslHandler = ctx.getPipeline().get(SslHandler.class);
        sslHandler.handshake();
                                    /*
        HashMap<String, Object> map = new HashMap<String, Object>();
        map.put("type","connect");
        map.put("ver", this.mobile.GetVer());
        ctx.getChannel().write(map);  */
    }

    @Override
    public void messageReceived(
            ChannelHandlerContext ctx, MessageEvent e) {
        System.err.println(e.getMessage());
    }

    @Override
    public void exceptionCaught(
            ChannelHandlerContext ctx, ExceptionEvent e) {
        logger.log(
                Level.WARNING,
                "Unexpected exception from downstream.",
                e.getCause());
        e.getChannel().close();
    }
}