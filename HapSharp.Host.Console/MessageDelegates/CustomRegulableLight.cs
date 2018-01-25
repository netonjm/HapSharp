using System;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;

namespace HapSharp.Host.Terminal.MessageDelegates
{
	class CustomRegulableLightMessageDelegate : MessageRegulableLightDelegate
	{
		bool actualValue;
		int brightness = 100;

		public CustomRegulableLightMessageDelegate (RegulableLightAccessory lightAccessory)
			: base (lightAccessory)
		{

		}

		protected override void OnChangePower (bool value)
		{
			actualValue = value;
			Console.WriteLine ($"[Net][{Accessory.Name}][Set] [{value}]");
		}

		protected override bool OnGetPower ()
		{
			return actualValue;
		}

		public override void OnIdentify ()
		{
			Console.WriteLine ($"[Net][{Accessory.Name}] Identified");
		}

		protected override int OnGetBrightness ()
		{
			return brightness;
		}

		protected override void OnChangeBrightness (int value)
		{
			Console.WriteLine ($"[Net][{Accessory.Name}][Brightness] [{value}]");
			brightness = value;
		}
	}
}
