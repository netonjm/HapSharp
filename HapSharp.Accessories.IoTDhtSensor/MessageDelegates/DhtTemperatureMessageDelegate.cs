
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class DhtTemperatureMessageDelegate : TemperatureMessageDelegate
	{
		public int Value => DhtService.Current.Temperature;

		int lastValue;

		public DhtTemperatureMessageDelegate(DhtTemperatureAccesory accessory) : base(accessory)
		{
			DhtService.Current.Start(accessory.GpioPin, accessory.DhtModel, accessory.Delay);
		}

		public override int OnGetMessageReceived()
		{
			var newValue = Value;
			if (newValue != lastValue) {
				OnValueChanged();
				lastValue = newValue;
			}
			return newValue;
		}
	}
}
