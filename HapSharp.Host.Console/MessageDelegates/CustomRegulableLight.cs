using System;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;

namespace HapSharp.Host.Terminal.MessageDelegates
{
    class CustomRegulableLightMessageDelegate : MessageRegulableLightDelegate
    {
        bool actualValue;
        int brightness = 100;

        public CustomRegulableLightMessageDelegate (RegulableLightAccessory lightAccessory, string topic = null)
            : base (lightAccessory, topic)
        {

        }

        protected override void OnChangePower (bool value)
        {
            actualValue = value;
            Console.WriteLine ("[Net]" + Accessory.Name + " ChangePower to: " + value);
        }

        protected override bool OnGetPower ()
        {
            return actualValue;
        }

        public override void OnIdentify ()
        {
            Console.WriteLine ("[Net]" + Accessory.Name + " identified!!");
        }

        protected override int OnGetBrightness ()
        {
            return brightness;
        }

        protected override void OnChangeBrightness (int value)
        {
            Console.WriteLine ("[Net]" + Accessory.Name + " OnChangeBrightness:" + value);
            brightness = value;
        }
    }
}
