using HapSharp.Accessories;
using IoTSharp.Components;

namespace HapSharp.MessageDelegates
{
	public class IoTMotionSensorMessageDelegate : MessageMotionSensorDelegate
	{
		readonly IoTSensor proximitySensor;

		public IoTMotionSensorMessageDelegate(MotionSensorAccessory sensor) : base(sensor)
		{
			proximitySensor = new IoTSensor(Connectors.GPIO22);
		}

		public override bool OnGetMessageReceived()
		{
			proximitySensor.Update();
			return proximitySensor.HasPresence;
		}

		public override void Dispose()
		{
			proximitySensor.Dispose();
		}
	}
}
