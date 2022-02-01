using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace SimpleHomeAssistant.ControlCenter.Messaging
{
    /// <summary>
    /// extension methods for MQTT messaging
    /// </summary>
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMqttTopicReceiver(this IServiceCollection services)
        {
            services.AddSingleton<IMqttClient>(provider =>
            {
                var mqttFactory = new MqttFactory();
                return mqttFactory.CreateMqttClient();
            });
            services.AddSingleton<MqttTopicReceiver>();
            return services;
        }

        public static WebApplication UseMqttTopicRecieverAndHandlers(this WebApplication app)
        {
            var provider = app.Services.CreateScope().ServiceProvider;
            var client = provider.GetRequiredService<IMqttClient>();
            var receiver = provider.GetRequiredService<MqttTopicReceiver>();
            var config = app.Configuration;

            // config client connection
            var clientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(config.GetValue<string>("MQTT:ServerAddress"), config.GetValue<int>("MQTT:ServerPort"))
                .Build();
            client.ConnectAsync(clientOptions);
            return app;
        }
    }
}