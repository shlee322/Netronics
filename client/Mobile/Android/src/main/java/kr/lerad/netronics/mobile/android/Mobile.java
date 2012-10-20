package kr.lerad.netronics.mobile.android;

import kr.lerad.netronics.mobile.android.push.Push;
import org.jboss.netty.bootstrap.ClientBootstrap;
import org.jboss.netty.channel.*;
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
import java.util.HashMap;
import java.util.concurrent.Executors;

public class Mobile {
    private ClientBootstrap bootstrap;
    private SSLContext clientContext;
    int ver;
    private String host;
    private int port;
    private String authFile;
    private Channel _channel;
    private HashMap<String, RecvOnListener> Listener = new HashMap<String, RecvOnListener>();

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
        clientContext.init(null, SecureTrustManagerFactory.getTrustManagers(), null);

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

    public void Run() throws InterruptedException {
        _channel = bootstrap.connect(new InetSocketAddress(host, port)).await().getChannel();
    }

    public void On(String type, RecvOnListener listener)
    {
        Listener.put(type, listener);
    }

    public void Call(String type, Object o)
    {
        if(!Listener.containsKey(type))
            return;
        Listener.get(type).On(this, o);
    }

    public void Emit(String type, Object o)
    {
        HashMap<String, Object> map = new HashMap<String, Object>();
        map.put("type","msg");
        map.put("name", type);
        map.put("arg", o);
        _channel.write(map);
    }

    public int GetVer()
    {
        return this.ver;
    }

    public void SetAuthFile(String path)
    {
        authFile = path;
    }

    public String GetAuthFile()
    {
        return authFile;
    }

    public void AddPush(Push push)
    {
        push.SetMobile(this);
        push.Run();
    }
}
