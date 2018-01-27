using System;
using HapSharp.Accessories;
using IoTSharp.Components;

namespace HapSharp.MessageDelegates
{
	public class IoTLightMessageDelegate : MessageLightDelegate
	{
		readonly IoTRelay relay;

		public IoTLightMessageDelegate(LightAccessory sensor) : base(sensor)
		{
			relay = new IoTRelay(Connectors.GPIO17);
		}

		protected override void OnChangePower(bool value)
		{
			relay.EnablePin(0, value);
			//Console.WriteLine($"[Net][IoTLight][Set] " + value);
		}

		protected override bool OnGetPower()
		{
			return relay.GetPinValue(0);
		}
	}
}
