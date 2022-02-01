namespace SimpleHomeAssistant.ControlCenter.Messaging
{
    public class VoiceActivityTopicHandler : IMqttTopicHandler
    {
        public string Topic => "sha/voice/recognized";

        public Task Handle(string topic, object payload)
        {
            throw new NotImplementedException();
        }
    }
}