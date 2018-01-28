
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class DhtTemperatureMessageDelegate : TemperatureMessageDelegate
	{
		public DhtTemperatureMessageDelegate(DhtTemperatureAccesory accessory) : base(accessory)
		{
			DhtService.Current.Start(accessory.GpioPin, accessory.DhtModel, accessory.Delay);
		}

		public override int OnGetMessageReceived()
		{
			return DhtService.Current.Temperature;
		}
	}
}
