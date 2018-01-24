using System;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;

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
            Console.WriteLine("[Net]" + Accessory.Name + " ChangePower to: " + value);
        }

        protected override bool OnGetPower()
        {
            return actualValue;
        }

        public override void OnIdentify ()
        {
            Console.WriteLine("[Net]" + Accessory.Name + " identified!!");
        }
    }
}
