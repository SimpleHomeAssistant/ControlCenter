﻿using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace SimpleHomeAssistant.ControlCenter.Messaging
{
    public class MqttTopicReceiver
    {
        private IMqttClient _mqttClient;
        private IMqttClientOptions _mqttClientOptions;
        private IEnumerable<IMqttTopicHandler> _mqttHandlers;
        private ILogger _logger;

        public MqttTopicReceiver(IOptions<TopicRecieverOptions> options, IEnumerable<IMqttTopicHandler> topicHandlers, ILogger<MqttTopicReceiver> logger)
        {
            _logger = logger;
            _mqttHandlers = topicHandlers.ToList();

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
    }
}