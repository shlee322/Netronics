package kr.lerad.netronics.mobile.android;

import org.jboss.netty.channel.*;
import org.jboss.netty.handler.ssl.SslHandler;

import java.io.*;
import java.util.HashMap;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;

class Handler extends SimpleChannelUpstreamHandler {
    private static final Logger logger = Logger.getLogger(
            Handler.class.getName());
    public static final String AuthDataFile = "AuthData";

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

        HashMap<String, Object> map = new HashMap<String, Object>();
        map.put("type","connect");
        map.put("ver", this.mobile.GetVer());

        if(mobile.GetAuthFile() != null)
        {
            File f = new File(mobile.GetAuthFile());
            if(f.exists())
            {
                Map<String, String> authData = new HashMap<String, String>();
                BufferedReader in = new BufferedReader(new FileReader(mobile.GetAuthFile()));
                authData.put("id", in.readLine());
                authData.put("key", in.readLine());
                map.put("auth", authData);
                in.close();
            }
        }

        ctx.getChannel().write(map);
    }

    @Override
    public void messageReceived(
            ChannelHandlerContext ctx, MessageEvent e) {
        System.err.println("netronics " + e.getMessage());

        Map<?, ?> map = (Map<?, ?>)e.getMessage();
        if(map.get("type").equals("auth_data") && mobile.GetAuthFile() != null)
        {
            File f = new File(mobile.GetAuthFile());
            if(f.exists())
                f.delete();

            try {
                BufferedWriter out = new BufferedWriter(new FileWriter(mobile.GetAuthFile()));
                out.write(map.get("id").toString());
                out.newLine();
                out.write(map.get("key").toString());
                out.newLine();
                out.flush();
                out.close();
            } catch (IOException e1) {
                e1.printStackTrace();
            }
        }
        else if(map.get("type").equals("msg"))
        {
            this.mobile.Call(map.get("name").toString(), map.get("arg"));
        }
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