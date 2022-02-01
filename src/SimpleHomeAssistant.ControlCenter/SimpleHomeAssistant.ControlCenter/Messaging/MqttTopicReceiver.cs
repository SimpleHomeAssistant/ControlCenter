using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace SimpleHomeAssistant.ControlCenter.Messaging
{
    public class MqttTopicReceiver
    {
        private IMqttClient _mqttClient;
        private IMqttClientOptions _mqttClientOptions;

        public MqttTopicReceiver(IOptions<TopicRecieverOptions> options)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(options.Value.ServerAddress, options.Value.ServerPort)
                .Build();
        }

        public void Start(IEnumerable<IMqttTopicHandler> topicHandlers)
        {
        }
    }
}