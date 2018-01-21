using HapSharp.Accessories;
using System;

namespace HapSharp.Host.Terminal
{
    class CustomLightMessageDelegate : MessageLightDelegate
    {
        bool actualValue;

        public CustomLightMessageDelegate(LightAccessory lightAccessory, string topic = null) 
            : base(lightAccessory, topic)
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


    class CustomRegulableLightMessageDelegate : MessageRegulableLightDelegate
    {
        bool actualValue;
        int brightness = 100;

        public CustomRegulableLightMessageDelegate (RegulableLightAccessory lightAccessory, string topic = null)
            : base(lightAccessory, topic)
        {

        }

        protected override void OnChangePower(bool value)
        {
            actualValue = value;
            Console.WriteLine("[Net]" + accessory.Name + " ChangePower to: " + value);
        }

        protected override bool OnGetPower()
        {
            return actualValue;
        }

        public override void OnIdentify()
        {
            Console.WriteLine("[Net]" + accessory.Name + " identified!!");
        }

        protected override int OnGetBrightness()
        {
            return brightness;
        }

        protected override void OnChangeBrightness(int value)
        {
            Console.WriteLine("[Net]" + accessory.Name + " OnChangeBrightness:" + value);
            brightness = value;
        }
    }
}
