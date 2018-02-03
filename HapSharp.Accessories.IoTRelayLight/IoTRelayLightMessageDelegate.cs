using System;
using HapSharp.Accessories;
using IoTSharp.Components;

namespace HapSharp.MessageDelegates
{
	public class IoTLightMessageDelegate : LightMessageDelegate
	{
		IoTRelay relay;

		readonly IoTLightAccessory sensor;
		public IoTLightMessageDelegate(IoTLightAccessory sensor) : base(sensor)
		{
			this.sensor = sensor;
		}

		public override void OnInitialize ()
		{
			relay = new IoTRelay (sensor.Connector);
		}

		public override void OnChangePower (bool value)
		{
			var resultValue = sensor.InverseSwitch ? !value : value;
			WriteLog ($"[On] {resultValue}");
			relay.EnablePin (0, resultValue);
		}

		public override bool OnGetPower ()
		{
			return relay.GetPinValue (0);
		}

		public override void Dispose()
		{
			relay.Dispose();
		}
	}
}
