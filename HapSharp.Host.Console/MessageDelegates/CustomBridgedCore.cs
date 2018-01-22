using System;
using HapSharp.Core.Accessories;
using HapSharp.Core.MessageDelegates;

namespace HapSharp.Host.Terminal.MessageDelegates
{
    class CustomBridgedCoreMessageDelegate : MessageBridgedCoreDelegate
    {
        public CustomBridgedCoreMessageDelegate (BridgedCore bridgedCoreAccessory) : base (bridgedCoreAccessory)
        {

        }

        public override void OnIdentify()
        {
            Console.WriteLine("[Net]" + accessory.Name + " identified!!");
        }
    }
}
