using MediatR;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace SimpleHomeAssistant.ControlCenter.Messaging
{
    public class MqttTopicProcessor
    {
        private IMqttClient _mqttClient;
        private IMqttClientOptions _mqttClientOptions;
        private IEnumerable<IMqttTopicHandler> _mqttHandlers;
        private ILogger _logger;
        private readonly IMediator _mediator;

        public MqttTopicProcessor(IOptions<TopicRecieverOptions> options, IEnumerable<IMqttTopicHandler> topicHandlers, ILogger<MqttTopicProcessor> logger, IMediator mediator)
        {
            _logger = logger;
            _mqttHandlers = topicHandlers.ToList();
            _mediator = mediator;
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(options.Value.ServerAddress, options.Value.ServerPort)
                .Build();
            InitMqttClient();
        }

        private void InitMqttClient()
        {
            _mqttClient.UseConnectedHandler(MqttConnectedHandler);
            _mqttClient.UseDisconnectedHandler(MqttDisconnectedHandler);
            _mqttClient.UseApplicationMessageReceivedHandler(MqttApplicaionMessageReceivedHandler);
        }

        private async Task MqttApplicaionMessageReceivedHandler(MqttApplicationMessageReceivedEventArgs arg)
        {
            _logger.LogInformation($"MQTT message received:{arg.ApplicationMessage.Topic}");
            var message = arg.ApplicationMessage;
            foreach (var mqttTopicHandler in _mqttHandlers)
            {
                // todo support wildcard
                if (mqttTopicHandler.Topic.Equals(message.Topic))
                {
                    var t = mqttTopicHandler.Handle(message.Topic, message.Payload);
                }
            }
        }

        private Task MqttDisconnectedHandler(MqttClientDisconnectedEventArgs arg)
        {
            _logger.LogWarning($"MQTT disconnected:{arg.Reason}");
            if (arg.Reason != MqttClientDisconnectReason.NotAuthorized && arg.Reason != MqttClientDisconnectReason.BadAuthenticationMethod)
            {
                _logger.LogInformation($"reconnecting MQTT after disconnection");
                _mqttClient.ReconnectAsync();
            }
            return Task.CompletedTask;
        }

        private Task MqttConnectedHandler(MqttClientConnectedEventArgs arg)
        {
            _logger.LogInformation($"MQTT connected. {arg.ConnectResult}");
            foreach (var mqttTopicHandler in _mqttHandlers)
            {
                _logger.LogDebug($"subscribing topic:{mqttTopicHandler.Topic}");
                _mqttClient.SubscribeAsync(mqttTopicHandler.Topic);
            }
            return Task.CompletedTask;
        }

        public void Start()
        {
            _logger.LogInformation($"connecting MQTT.");
            _mqttClient.ConnectAsync(_mqttClientOptions);
        }

        public Task Publish(string topic, string payload, CancellationToken cancellationToken)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(0)
                .WithPayload(payload)
                .Build();
            return _mqttClient.PublishAsync(message, cancellationToken);
        }
    }
}