using System;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;

namespace HapSharp.Host.Terminal.MessageDelegates
{
	class CustomTemperatureMessageDelegate : GetMessageDelegate
	{
		Random rnd = new Random (DateTime.Now.Millisecond);

		public CustomTemperatureMessageDelegate (TemperatureAccessory accessory) : base (accessory)
		{
		}

		public override int OnGetMessageReceived ()
		{
			var calculated = rnd.Next (20, 50);
			Console.WriteLine ($"[Net] Temperature: {calculated}");
			return calculated;
		}


		public override void OnIdentify ()
		{
			Console.WriteLine ("[Net]" + Accessory.Name + " identified!!");
		}
	}
}
