using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class DhtHumidityMessageDelegate : HumidityMessageDelegate
	{
		public DhtHumidityMessageDelegate(DhtHumidityAccesory accessory) : base(accessory)
		{
			DhtService.Current.Start(accessory.GpioPin, accessory.DhtModel, accessory.Delay);
		}

		public override int OnGetMessageReceived()
		{
			return DhtService.Current.Humidity;
		}
	}
}
