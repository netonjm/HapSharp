using System;
using HapSharp.Accessories;

namespace HapSharp.Host.Terminal
{
    class CustomBridgedCoreMessageDelegate : MessageBridgedCoreDelegate
    {
        public CustomBridgedCoreMessageDelegate (CustomBridgedCoreAccessory bridgedCoreAccessory) : base (bridgedCoreAccessory)
        {

        }

        public override void OnIdentify()
        {
            Console.WriteLine("[Net]" + accessory.Name + " identified!!");
        }
    }

    class CustomBridgedCoreAccessory : BridgedCore
    {
        public CustomBridgedCoreAccessory (string name = null, string username = null) : base (name, username)
        {
            Manufacturer = "NetOnJM";
            Model = "BridgedCore Model";
            SerialNumber = "333-333";
        }

    }
}
