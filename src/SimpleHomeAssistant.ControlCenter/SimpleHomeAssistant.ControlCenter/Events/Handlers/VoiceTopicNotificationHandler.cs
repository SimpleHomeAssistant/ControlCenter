using MediatR;
using Microsoft.VisualBasic;
using SimpleHomeAssistant.ControlCenter.Events.Notifications;
using SimpleHomeAssistant.ControlCenter.Messaging;

namespace SimpleHomeAssistant.ControlCenter.Events.Handlers
{
    public class VoiceTopicNotificationHandler : INotificationHandler<VoiceTopicNotification>
    {
        private readonly MqttTopicProcessor _topicProcessor;

        public VoiceTopicNotificationHandler(MqttTopicProcessor topicProcessor)
        {
            _topicProcessor = topicProcessor;
        }

        public Task Handle(VoiceTopicNotification notification, CancellationToken cancellationToken)
        {
            // write command keywords into database

            if (notification.Keywords.Contains("打开") && notification.Keywords.Contains("电视"))
            {
                _topicProcessor.Publish("sha/controller/command/ir", "1DFF", cancellationToken);
            }
            return Task.CompletedTask;
        }
    }
}