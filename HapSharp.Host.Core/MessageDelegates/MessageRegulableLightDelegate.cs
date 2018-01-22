using HapSharp.Core.Accessories;

namespace HapSharp.Core.MessageDelegates
{
    public abstract class MessageRegulableLightDelegate : MessageLightDelegate
    {
        const string TopicSetBrightness = "set/brightness";
        const string TopicGetBrightness = "get/brightness";

        protected MessageRegulableLightDelegate (RegulableLightAccessory accessory, string topic = null) : base (accessory)
        {

        }

        internal override void OnMessageReceived (string topic, string message)
        {
            if (message.StartsWith (TopicSetBrightness + "/")) {
                OnChangeBrightness (message.Substring ((TopicSetBrightness + "/").Length).ToInt ());
            } else if (message == TopicGetBrightness) {
                var current = OnGetBrightness ();
                OnSendMessage (topic + "/" + ReceiveTopicNode, message + "/" + current);
            } else {
                base.OnMessageReceived (topic, message);
            }
        }

        public override string GetTemplate ()
        {
            return base.GetTemplate ()
                       .Replace (GetTemplateTagId (accessory.Prefix, nameof (TopicGetBrightness)), TopicGetBrightness)
                       .Replace (GetTemplateTagId (accessory.Prefix, nameof (TopicSetBrightness)), TopicSetBrightness);
        }

        protected abstract int OnGetBrightness ();
        protected abstract void OnChangeBrightness (int value);
    }
}
