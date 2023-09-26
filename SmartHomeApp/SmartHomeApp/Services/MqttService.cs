using DotNetty.Buffers;
using DotNetty.Codecs.Mqtt;
using DotNetty.Codecs.Mqtt.Packets;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

public class MqttService
{
    private IEventLoopGroup group;
    private IChannel channel;

    public event Action<string> MessageReceived; // Event, das ausgelöst wird, wenn eine Nachricht empfangen wird

    public MqttService()
    {
        SetupMqttClient();
    }

    private async void SetupMqttClient()
    {
        group = new MultithreadEventLoopGroup();

        var bootstrap = new Bootstrap()
            .Group(group)
            .Channel<TcpSocketChannel>()
            .Option(ChannelOption.TcpNodelay, true)
            .Handler(new ActionChannelInitializer<ISocketChannel>(ch =>
            {
                var pipeline = ch.Pipeline;
                pipeline.AddLast("encoder", new MqttEncoder());
                pipeline.AddLast("decoder", new MqttDecoder(true, 256 * 1024));
                pipeline.AddLast("handler", new MqttClientHandler(this));
            }));

        try
        {
            channel = await bootstrap.ConnectAsync("broker.hivemq.com", 1883); // Zum Beispiel ein öffentlicher Broker
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
        }
    }

    public async Task Subscribe(string topic)
    {
        if (channel != null && channel.Open)
        {
            var subscribePacket = new SubscribePacket(1, new SubscriptionRequest(topic, QualityOfService.AtMostOnce));
            await channel.WriteAndFlushAsync(subscribePacket);
        }
    }

    public void Publish(string topic, string message)
    {
        if (channel != null && channel.Open)
        {
            var packet = new PublishPacket(QualityOfService.AtMostOnce, false, false)
            {
                TopicName = topic,
                Payload = Unpooled.WrappedBuffer(Encoding.UTF8.GetBytes(message))
            };

            channel.WriteAndFlushAsync(packet);
        }
    }

    class MqttClientHandler : ChannelHandlerAdapter
    {
        private readonly MqttService mqttService;

        public MqttClientHandler(MqttService mqttService)
        {
            this.mqttService = mqttService;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Debug.WriteLine("Message received in ChannelRead"); // Hinzugefügt für Debugging
            if (message is PublishPacket packet)
            {
                string payload = packet.Payload.ToString(Encoding.UTF8);
                mqttService.MessageReceived?.Invoke(payload);
            }
            else
            {
                Debug.WriteLine($"Unhandled message type: {message.GetType().Name}"); // Hinzugefügt für Debugging
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Debug.WriteLine($"Error: {exception.Message}"); // Geändert von Console.WriteLine
        }
    }

}
