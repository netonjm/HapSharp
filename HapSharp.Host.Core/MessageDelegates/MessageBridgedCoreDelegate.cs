using System;
using HapSharp.Core.Accessories;

namespace HapSharp.Core.MessageDelegates
{
    public abstract class MessageBridgedCoreDelegate : MessageDelegate
    {
        protected MessageBridgedCoreDelegate (BridgedCore accessory, string topic = null) : base (accessory)
        {

        }

        public override void OnIdentify ()
        {
            Console.WriteLine ("[Net][MessageBridgedCore] identified!!");
        }

        public override string GetTemplate ()
        {
            var port = ((BridgedCore)accessory).Port.ToString ();
            return base.GetTemplate ()
                       .Replace (GetTemplateTagId (accessory.Prefix, nameof (BridgedCore.Port)), port);
        }
    }
}

