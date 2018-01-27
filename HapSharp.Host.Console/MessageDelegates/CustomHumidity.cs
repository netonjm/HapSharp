using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	class CustomHumidityMessageDelegate : HumidityMessageDelegate
	{
		public CustomHumidityMessageDelegate (HumidityAccessory accessory) : base (accessory)
		{
		}

		Random rnd = new Random (DateTime.Now.Millisecond);
		public override int OnGetMessageReceived ()
		{
			var calculated = rnd.Next (20, 50);
			Console.WriteLine ($"[Net][{Accessory.Name}][Get] {calculated}");
			return calculated;
		}

		public override void OnIdentify ()
		{
			Console.WriteLine ($"[Net][{Accessory.Name}] Identified.");
		}
	}
}
