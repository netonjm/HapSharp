using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class BridgedCoreMessageDelegate : MessageDelegate
	{
		public BridgedCoreMessageDelegate (BridgedCore accessory) : base (accessory)
		{

		}
	}
}

