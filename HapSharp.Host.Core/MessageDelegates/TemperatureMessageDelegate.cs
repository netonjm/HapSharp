using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class TemperatureMessageDelegate : GetMessageDelegate<int>
	{
		public TemperatureMessageDelegate (TemperatureAccessory accessory) : base (accessory)
		{
		}
	}
}
