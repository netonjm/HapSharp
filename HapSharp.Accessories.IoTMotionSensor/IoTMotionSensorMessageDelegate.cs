using HapSharp.Accessories;
using IoTSharp.Components;

namespace HapSharp.MessageDelegates
{
	public class IoTMotionSensorMessageDelegate : MotionSensorMessageDelegate
	{
		IoTSensor proximitySensor;

		readonly IoTMotionSensorAccessory sensor;

		public override bool Value => proximitySensor.HasPresence;

		bool lastValue;

		public IoTMotionSensorMessageDelegate(IoTMotionSensorAccessory sensor) : base(sensor)
		{
			this.sensor = sensor;
		}

		public override void OnInitialize()
		{
			proximitySensor = new IoTSensor(sensor.Connector);
		}

		public override bool OnGetMessageReceived()
		{
			proximitySensor.Update();
			var newValue = Value;
			if (newValue != lastValue)
			{
				OnValueChanged();
				lastValue = newValue;
			}
			return newValue;
		}

		public override void Dispose()
		{
			proximitySensor.Dispose();
		}
	}
}
