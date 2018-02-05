using System;
using HapSharp.Accessories;
using IoTSharp.Components;

namespace HapSharp.MessageDelegates
{
	public class IoTLightMessageDelegate : LightMessageDelegate
	{
		readonly IoTLightAccessory sensor;
		IoTRelay relay;

		public bool Value => relay.GetPinValue(0);
		bool lastValue;

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
			OnValueChanged();
		}

		public override bool OnGetPower ()
		{
			var newValue = Value;
			if (newValue != lastValue) {
				OnValueChanged();
				lastValue = newValue;
			}
			return newValue;
		}

		public override void Dispose()
		{
			relay.Dispose();
		}
	}
}
