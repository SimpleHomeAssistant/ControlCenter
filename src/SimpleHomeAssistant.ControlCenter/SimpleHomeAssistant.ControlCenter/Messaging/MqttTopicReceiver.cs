using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace SimpleHomeAssistant.ControlCenter.Messaging
{
    public class MqttTopicReceiver
    {
        private IMqttClient _mqttClient;

        public MqttTopicReceiver(IMqttClient mqttClient)
        {
            _mqttClient = mqttClient;
        }

        public void Start(IEnumerable<IMqttTopicHandler> topicHandlers)
        {
        }
    }
}