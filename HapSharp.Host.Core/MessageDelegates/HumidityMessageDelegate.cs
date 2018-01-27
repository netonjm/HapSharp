using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class HumidityMessageDelegate : GetMessageDelegate<int>
	{
		public HumidityMessageDelegate (HumidityAccessory accessory) : base (accessory)
		{
		}
	}
}
