using System;
using HapSharp.MessageDelegates;

namespace HapSharp.Accessories
{
	public abstract class MessageTemperatureDelegate : GetMessageDelegate
	{
		Random rnd = new Random (DateTime.Now.Millisecond);

		protected MessageTemperatureDelegate (TemperatureAccessory accessory) : base (accessory)
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
			Console.WriteLine ($"[Net][{Accessory.Name}] Identified");
		}
	}
}
