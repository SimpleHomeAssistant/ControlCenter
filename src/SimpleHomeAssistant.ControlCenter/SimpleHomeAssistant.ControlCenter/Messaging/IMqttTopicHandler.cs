namespace SimpleHomeAssistant.ControlCenter.Messaging
{
    public interface IMqttTopicHandler
    {
        /// <summary>
        /// the interested MQTT topic
        /// </summary>
        string Topic { get; set; }

        /// <summary>
        /// handle and process the command
        /// </summary>
        /// <param name="command">received MQTT topic</param>
        /// <param name="payload">payload</param>
        /// <returns></returns>
        Task Handle(string topic, object payload);
    }
}