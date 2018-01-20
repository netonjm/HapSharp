using System;
using HapNet.Accessories;

namespace HapSharp.Host.Terminal
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var lightAccessory = new LightAccessory()
            {
                Id = "12:A2:33:A4:55:16",
                Name = "MyLight",
                PinCode = "031-45-154",
                Manufacturer = "NetOnJM",
                Model = "Model",
                SerialNumber = "123-456"
            };

            var lightMessageDelegate = new CustomLightMessageDelegate (lightAccessory);

            var session = new HapSession ();
            session.Connect (StaticResources.BrokerAddress);
            session.Add (lightMessageDelegate);

            Console.WriteLine ("Press a key to exit.");
            Console.ReadKey ();
        }
    }
}
