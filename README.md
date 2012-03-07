Netronics aim to simple and high-performance general server framework on .net framework.

By using this framework, easily can develop server.

In addition, using the Template can simply implement the services such as http.

## Simple Echo Server Example
```csharp
// Server Starting Point
class Program
{
    private static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

    static void Main(string[] args)
    {
        var properties = new Properties(); //Server Properties Class
        properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 7777)); //set up server ip and port
        properties.SetChannelFactoryOption(factory => SetFatoryOption((ChannelFactory)factory)); //set up client create factory Options

        var netronics = new Netronics.Netronics(properties);
        netronics.Start(); //Server Start

        ExitEvent.WaitOne();
    }

    private static void SetFatoryOption(ChannelFactory factory)
    {
		PacketEncoder encoder = new PacketEncoder();
        factory.SetProtocol(() => new ModifiableProtocol(encoder: encoder, decoder: encoder));
        factory.SetHandler(() => new Handler());
    }
}

// Client Handler
class Handler : IChannelHandler
{
    public void Connected(IChannel channel)
    {
    }

    public void Disconnected(IChannel channel)
    {
    }

    public void MessageReceive(IChannel channel, dynamic message)
    {
        channel.SendMessage(message);
    }
}

// PacketEncoder, Decoder
class PacketEncoder : IPacketEncoder, IPacketDecoder
{
    public PacketBuffer Encode(IChannel channel, dynamic data)
    {
        if (data.GetType() != typeof(byte[]))
            return null;
        byte[] bytes = (byte[]) data;
        PacketBuffer buffer = new PacketBuffer();
        buffer.Write(bytes, 0, bytes.Length);
        return buffer;
    }

    public dynamic Decode(IChannel channel, PacketBuffer buffer)
    {
        buffer.BeginBufferIndex();
        if (buffer.AvailableBytes() < 1)
        {
            buffer.ResetBufferIndex();
            return null;
        }
        var data = new byte[buffer.AvailableBytes()];
        buffer.ReadBytes(data);
        buffer.EndBufferIndex();
        return data;
    }
}
```

## Download
* **[Project Site](https://github.com/shlee322/Netronics)**
* **[Bin](https://github.com/shlee322/Netronics/downloads)**
* Source
"git clone git://github.com/shlee322/Netronics.git"

[Release notes for major releases](https://github.com/shlee322/Netronics/wiki/Release-Notes)

## Getting Started
Online tutorials that walks you through developing is available here:

* **[Read the documentation on the Netronics Wiki](https://github.com/shlee322/Netronics/wiki)**
* [Example](https://github.com/shlee322/Netronics/tree/master/example)

## News
* [Facebook Page](https://www.facebook.com/pages/Netronics/340884399263648)
* Google Group

## Q&A
* [Netronics English Group](https://groups.google.com/group/netronics-en)
* [Netronics Korean Group](https://groups.google.com/group/netronics-ko)

## Discussion
* [Netronics Developer Group](https://groups.google.com/group/netronics-dev)

## Contributors
A big thanks to GitHub and all of Netronics contributors:

- [shlee322](https://github.com/shlee322)(SangHyuck, Lee)