using HapSharp.Core.Accessories;

namespace HapSharp.Core.MessageDelegates
{
    public abstract class MessageTemperatureDelegate : MessageDelegate
    {
        const string TopicGet = "get";
        const string Interval = "Interval";

        protected MessageTemperatureDelegate (TemperatureAccessory accessory) : base (accessory)
        {
        }

        protected override void OnMessageReceived (string topic, string message)
        {
            if (message == TopicGet) {
                var value = OnGetTemperature ();
                OnSendMessage (topic, message, value);
            } else {
                throw new System.NotImplementedException (message);
            }
        }

        public override string GetTemplate ()
        {
            var interval = ((TemperatureAccessory)accessory).Interval.ToString ();
            return base.GetTemplate ()
                       .Replace (GetTemplateTagId (accessory.Prefix, nameof (TemperatureAccessory.Interval)), interval)
                       .Replace (GetTemplateTagId (accessory.Prefix, nameof (TopicGet)), TopicGet);
        }

        protected abstract int OnGetTemperature ();

    }
}
