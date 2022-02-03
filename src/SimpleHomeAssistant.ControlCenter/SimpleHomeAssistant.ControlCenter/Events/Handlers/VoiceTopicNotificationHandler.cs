using MediatR;
using SimpleHomeAssistant.ControlCenter.Events.Notifications;
using SimpleHomeAssistant.ControlCenter.Messaging;

namespace SimpleHomeAssistant.ControlCenter.Events.Handlers
{
    public class VoiceTopicNotificationHandler : INotificationHandler<VoiceTopicNotification>
    {
        private MqttTopicProcessor _topicProcessor;

        public VoiceTopicNotificationHandler()
        {
        }

        public Task Handle(VoiceTopicNotification notification, CancellationToken cancellationToken)
        {
            // write command keywords into database
            throw new NotImplementedException();
        }
    }
}