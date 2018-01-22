using HapSharp.Accessories;
using System;

namespace HapSharp.Host.Terminal
{
    class CustomTemperatureMessageDelegate : MessageTemperatureDelegate
    {
        public CustomTemperatureMessageDelegate(TemperatureAccessory accessory) : base (accessory)
        {
        }

        Random rnd = new Random(DateTime.Now.Millisecond);
        protected override int OnGetTemperature()
        {
            var calculated = rnd.Next(20, 50);
            Console.WriteLine($"[Net] Temperature: {calculated}");
            return calculated;
        }

        public override void OnIdentify()
        {
            Console.WriteLine("[Net]" + accessory.Name + " identified!!");
        }
    }
}
