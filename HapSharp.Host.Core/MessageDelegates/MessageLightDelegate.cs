using System;
using HapSharp.Core.Accessories;

namespace HapSharp.Core.MessageDelegates
{
    public abstract class MessageLightDelegate : MessageDelegate
    {
        const string TopicSetOn = "set/on";
        const string TopicGetOn = "get/on";


        protected MessageLightDelegate (LightAccessory accessory) : base (accessory)
        {

        }

        protected override void OnMessageReceived (string topic, string message)
        {
            if (message.StartsWith (TopicSetOn + "/")) {
                OnChangePower (message.Substring ((TopicSetOn + "/").Length).ToBoolean ());
            } else if (message == TopicGetOn) {
                var current = OnGetPower () ? "true" : "false";
                OnSendMessage (topic, message, current);
            } else {
                throw new NotImplementedException (message);
            }
        }

        public override string GetTemplate ()
        {
            return base.GetTemplate ()
                       .Replace (GetTemplateTagId (accessory.Prefix, nameof (TopicGetOn)), TopicGetOn)
                       .Replace (GetTemplateTagId (accessory.Prefix, nameof (TopicSetOn)), TopicSetOn);
        }

        public override void OnIdentify ()
        {
            Console.WriteLine ("[Net]" + accessory.Name + " identified!!");
        }

        protected abstract bool OnGetPower ();


        protected abstract void OnChangePower (bool value);

    }
}
