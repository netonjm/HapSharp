using System;
using HapNet.Accessories;

namespace HapSharp.Host.Terminal
{
    class CustomLightMessageDelegate : MessageLightDelegate
    {
        bool actualValue;

        public CustomLightMessageDelegate (LightAccessory accessory) : base(accessory)
        {

        }

        protected override void OnChangePower(bool value)
        {
            actualValue = value;
            Console.WriteLine("Power changed to: " + value);
        }

        protected override bool OnGetPower()
        {
            return actualValue;
        }
    }
}
