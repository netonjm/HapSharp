using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class MessageHumidityDelegate : GetMessageDelegate<int>
	{
		public MessageHumidityDelegate (HumidityAccessory accessory) : base (accessory)
		{
		}
	}
}
