using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	class CustomMotionSensorMessageDelegate : MotionSensorMessageDelegate
	{
		bool value;
		Random rnd = new Random();
		public override bool Value { get; }

		public CustomMotionSensorMessageDelegate(MotionSensorAccessory accessory) : base(accessory)
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
