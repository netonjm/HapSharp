using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class MessageBridgedCoreDelegate : MessageDelegate
	{
		protected MessageBridgedCoreDelegate (BridgedCore accessory, string topic = null) : base (accessory)
		{

		}
	}
}

