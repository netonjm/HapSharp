namespace HapSharp.Accessories
{
    public class TemperatureAccessory : Accessory
    {
        public int Temperature { get; set; } = 30;
        public int Interval { get; set; } = 6000;
        public override string Template => "Temperature_accessory.js";

        public TemperatureAccessory(string name = null, string username = null) : base(name, username)
        {

        }
    }

    public abstract class MessageTemperatureDelegate : MessageDelegate
    {
        const string TopicGet = "get";
        const string Interval = "Interval";

        protected MessageTemperatureDelegate(TemperatureAccessory accessory) : base(accessory)
        {
        }

        internal override void OnMessageReceived(string topic, string message)
        {
            if (message == TopicGet)
            {
                var value = OnGetTemperature();
                OnSendMessage(topic + "/" + ReceiveTopicNode, message + "/" + value.ToString());
            }
            else
            {
                throw new System.NotImplementedException(message);
            }
        }

        public override string GetTemplate()
        {
            var interval = ((TemperatureAccessory)accessory).Interval.ToString();
            return base.GetTemplate()
                       .Replace(GetTemplateTagId(accessory.Prefix, nameof(TemperatureAccessory.Interval)), interval)
                       .Replace(GetTemplateTagId(accessory.Prefix, nameof(TopicGet)), TopicGet);
        }

        protected abstract int OnGetTemperature();

    }
}
