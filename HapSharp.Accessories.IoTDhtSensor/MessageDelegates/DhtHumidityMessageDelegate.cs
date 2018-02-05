using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class DhtHumidityMessageDelegate : HumidityMessageDelegate
	{
		public int Value => DhtService.Current.Humidity;
		int lastValue;
		public DhtHumidityMessageDelegate(DhtHumidityAccesory accessory) : base(accessory)
		{
			DhtService.Current.Start(accessory.GpioPin, accessory.DhtModel, accessory.Delay);
		}

		public override int OnGetMessageReceived()
		{
			var newValue = Value;
			if (newValue != lastValue)
			{
				OnValueChanged();
				lastValue = newValue;
			}
			return Value;
		}
	}
}
