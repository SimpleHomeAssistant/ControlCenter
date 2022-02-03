using System.Text;
using MediatR;
using SimpleHomeAssistant.ControlCenter.Events.Notifications;

namespace SimpleHomeAssistant.ControlCenter.Messaging
{
    public class VoiceActivityTopicHandler : IMqttTopicHandler
    {
        private readonly IMediator _mediator;

        public VoiceActivityTopicHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string Topic => "sha/voice/recognized";

        public Task Handle(string topic, byte[] payload)
        {
            var payloadContent = Encoding.UTF8.GetString(payload);
            var keywords = payloadContent.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            var notification = new VoiceTopicNotification()
            {
                Keywords = keywords,
                Topic = topic,
                RawContent = payloadContent
            };
            _mediator.Publish(notification); // no need to await.
            return Task.CompletedTask;
        }
    }
}