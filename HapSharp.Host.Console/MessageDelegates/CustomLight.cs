using System;
using HapSharp.Core.Accessories;
using HapSharp.Core.MessageDelegates;

namespace HapSharp.Host.Terminal.MessageDelegates
{
    class CustomLightMessageDelegate : MessageLightDelegate
    {
        bool actualValue;

        public CustomLightMessageDelegate (LightAccessory lightAccessory) 
            : base (lightAccessory)
        {

        }

        protected override void OnChangePower (bool value)
        {
            actualValue = value;
            Console.WriteLine("[Net]" + accessory.Name + " ChangePower to: " + value);
        }

        protected override bool OnGetPower()
        {
            return actualValue;
        }

        public override void OnIdentify ()
        {
            Console.WriteLine("[Net]" + accessory.Name + " identified!!");
        }
    }
}
