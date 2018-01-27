using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	class CustomTemperatureMessageDelegate : TemperatureMessageDelegate
	{
		Random rnd = new Random (DateTime.Now.Millisecond);

		public CustomTemperatureMessageDelegate (TemperatureAccessory accessory) : base (accessory)
		{
		}

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
