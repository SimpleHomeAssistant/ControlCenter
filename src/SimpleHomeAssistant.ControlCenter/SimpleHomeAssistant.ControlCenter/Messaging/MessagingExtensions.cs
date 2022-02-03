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
        public static IServiceCollection AddMqttTopicReceiverAndHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IMqttClient>(provider =>
            {
                var mqttFactory = new MqttFactory();
                return mqttFactory.CreateMqttClient();
            });
            services.AddSingleton<MqttTopicProcessor>();

            services.AddSingleton<IMqttTopicHandler, VoiceActivityTopicHandler>();

            return services;
        }

        public static WebApplication UseMqttTopicReceiver(this WebApplication app)
        {
            var provider = app.Services.CreateScope().ServiceProvider;
            var receiver = provider.GetRequiredService<MqttTopicProcessor>();
            receiver.Start();

            return app;
        }
    }
}