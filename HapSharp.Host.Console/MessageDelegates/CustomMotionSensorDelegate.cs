using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	class CustomMotionSensorDelegate : MessageMotionSensorDelegate
	{
		bool value;
		Random rnd = new Random();

		public CustomMotionSensorDelegate(MotionSensorAccessory accessory) : base(accessory)
		{

		}

		public override bool OnGetMessageReceived()
		{
			var newValue = rnd.Next(100) <= 20;
			if (value != newValue)
			{
				value = newValue;
				Console.WriteLine($"[Net][{Accessory.Name}][Get] {newValue}");
			}

			return value;
		}

		public override void OnIdentify()
		{
			Console.WriteLine($"[Net][{Accessory.Name}] Identified");
		}
	}
}
