using MediatR;

namespace SimpleHomeAssistant.ControlCenter.Events.Notifications
{
    public class VoiceTopicNotification : INotification
    {
        public string Topic { get; set; }
        public int QoS { get; set; }
        public List<string> Keywords { get; set; }
        public string RawContent { get; set; }
    }
}