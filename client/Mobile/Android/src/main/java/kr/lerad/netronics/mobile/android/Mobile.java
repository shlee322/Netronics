package kr.lerad.netronics.mobile.android;

import org.jboss.netty.bootstrap.ClientBootstrap;
import org.jboss.netty.channel.ChannelFactory;
import org.jboss.netty.channel.ChannelPipeline;
import org.jboss.netty.channel.ChannelPipelineFactory;
import org.jboss.netty.channel.Channels;
import org.jboss.netty.channel.socket.nio.NioClientSocketChannelFactory;
import org.jboss.netty.handler.codec.string.StringDecoder;
import org.jboss.netty.handler.codec.string.StringEncoder;
import org.jboss.netty.handler.ssl.SslHandler;

import javax.net.ssl.KeyManagerFactory;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLEngine;
import javax.net.ssl.TrustManagerFactory;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.lang.Object;
import java.lang.String;
import java.net.InetSocketAddress;
import java.security.*;
import java.security.cert.CertificateException;
import java.util.concurrent.Executors;

public class Mobile {
    private ClientBootstrap bootstrap;
    private SSLContext clientContext;
    int ver;
    private String host;
    private int port;

    public Mobile(int ver, String host, int port) throws NoSuchAlgorithmException, KeyManagementException {
        this.ver = ver;
        this.host = host;
        this.port = port;

        ChannelFactory factory =
                new NioClientSocketChannelFactory(
                        Executors.newCachedThreadPool(),
                        Executors.newCachedThreadPool());

        this.bootstrap = new ClientBootstrap(factory);

        clientContext = SSLContext.getInstance("SSLv3");
        clientContext.init(null, SecureChatTrustManagerFactory.getTrustManagers(), null);

        this.bootstrap.setPipelineFactory(new ChannelPipelineFactory() {
                public ChannelPipeline getPipeline() {
                    ChannelPipeline pipeline = Channels.pipeline();
                    SSLEngine engine = Mobile.this.clientContext.createSSLEngine();
                    engine.setUseClientMode(true);
                    pipeline.addLast("ssl", new SslHandler(engine));
                    pipeline.addLast("decoder", new PacketDecoder());
                    pipeline.addLast("encoder", new PacketEncoder());
                    pipeline.addLast("handler", new Handler(Mobile.this));
                    return pipeline;
                }
            });
    }

    public void Run()
    {
        bootstrap.connect(new InetSocketAddress(host, port));
    }

    public void On(String type, Object o)
    {
    }

    public void Emit(String type, Object o)
    {
    }

    public int GetVer()
    {
        return this.ver;
    }
}
