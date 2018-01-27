using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class MessageTemperatureDelegate : GetMessageDelegate<int>
	{
		public MessageTemperatureDelegate (TemperatureAccessory accessory) : base (accessory)
		{
		}
	}
}
