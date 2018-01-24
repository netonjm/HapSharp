using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
    public abstract class MessageRegulableLightDelegate : MessageLightDelegate
    {
        const string TopicSetBrightness = "set/brightness";
        const string TopicGetBrightness = "get/brightness";

        protected MessageRegulableLightDelegate (RegulableLightAccessory accessory, string topic = null) : base (accessory)
        {

        }

        protected override void OnMessageReceived (string topic, string message)
        {
            if (message.StartsWith (TopicSetBrightness + "/")) {
                OnChangeBrightness (message.Substring ((TopicSetBrightness + "/").Length).ToInt ());
            } else if (message == TopicGetBrightness) {
                var current = OnGetBrightness ();
                OnSendMessage (topic, message, current);
            } else {
                base.OnMessageReceived (topic, message);
            }
        }

        public override string OnReplaceTemplate (string template)
        {
            return base.OnReplaceTemplate (template)
                       .Replace (Accessory.GetTemplateTagId (nameof (TopicGetBrightness)), TopicGetBrightness)
                       .Replace (Accessory.GetTemplateTagId (nameof (TopicSetBrightness)), TopicSetBrightness);
        }

        protected abstract int OnGetBrightness ();
        protected abstract void OnChangeBrightness (int value);
    }
}
