using System;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;

namespace HapSharp.Host.Terminal.MessageDelegates
{
	class CustomBridgedCoreMessageDelegate : MessageBridgedCoreDelegate
	{
		public CustomBridgedCoreMessageDelegate (BridgedCore bridgedCoreAccessory) : base (bridgedCoreAccessory)
		{

		}

		public override void OnIdentify ()
		{
			Console.WriteLine ($"[Net][{Accessory.Name}] Identified.");
		}
	}
}
