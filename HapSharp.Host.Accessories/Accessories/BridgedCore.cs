using System;
namespace HapSharp.Accessories
{
    public class BridgedCore : Accessory
    {
        internal virtual int Port => ResourcesService.ServicePort;

        public override string Template => "BridgedCore.js";

        public BridgedCore (string name = null, string username = null) : base (name, username)
        {
            
        }
    }

    public abstract class MessageBridgedCoreDelegate : MessageDelegate
    {
        protected MessageBridgedCoreDelegate(BridgedCore accessory, string topic = null) : base(accessory, topic ?? "/home/bridgedcore")
        {

        }

        public override void OnIdentify()
        {
            Console.WriteLine("[Net][MessageBridgedCore] identified!!");
        }

        public override string GetTemplate()
        {
            var port = ((BridgedCore)accessory).Port.ToString();
            return base.GetTemplate()
                       .Replace(GetTemplateTagId(accessory.Prefix, nameof(BridgedCore.Port)), port);
        }
    }
}

