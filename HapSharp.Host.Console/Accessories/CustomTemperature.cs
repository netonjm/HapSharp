using HapSharp.Accessories;
using System;

namespace HapSharp.Host.Terminal
{
    class CustomTemperatureMessageDelegate : MessageTemperatureDelegate
    {
        public CustomTemperatureMessageDelegate(TemperatureAccessory accessory, string topic = null) : base(accessory, topic)
        {
        }

        Random rnd = new Random(DateTime.Now.Millisecond);
        protected override int OnGetTemperature()
        {
            var calculated = rnd.Next(20, 50);
            Console.WriteLine($"[Net] Query Temperature: {calculated}");
            return calculated;
        }

        public override void OnIdentify()
        {
            Console.WriteLine("[Net]" + accessory.Name + " identified!!");
        }
    }
}
