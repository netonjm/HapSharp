using System;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;

namespace HapSharp.Accessories
{
	public abstract class MessageHumidityDelegate : GetMessageDelegate
	{
		Random rnd = new Random (DateTime.Now.Millisecond);

		protected MessageHumidityDelegate (HumidityAccessory accessory) : base (accessory)
		{
		}

		public override int OnGetMessageReceived ()
		{
			var calculated = rnd.Next (20, 50);
			Console.WriteLine ($"[Net] Humidity: {calculated}");
			return calculated;
		}

		public override void OnIdentify ()
		{
			Console.WriteLine ("[Net]" + Accessory.Name + " identified!!");
		}
	}
}
