using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	class CustomLightMessageDelegate : LightMessageDelegate
	{
		bool actualValue;

		public CustomLightMessageDelegate (LightAccessory lightAccessory)
			: base (lightAccessory)
		{

		}

		public override void OnChangePower (bool value)
		{
			actualValue = value;
			Console.WriteLine ($"[Net][{Accessory.Name}][Set] " + value);
		}

		public override bool OnGetPower ()
		{
			return actualValue;
		}

		public override void OnIdentify ()
		{
			Console.WriteLine ($"[Net][{Accessory.Name}] Identified.");
		}
	}
}
