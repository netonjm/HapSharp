using IoTSharp.Components;

namespace HapSharp.Accessories
{
	public class IoTMotionSensorAccessory : MotionSensorAccessory
	{
		public Connectors Connector { get; set; }

		public IoTMotionSensorAccessory(string name = null, string username = null) : base(name, username)
		{
		}
	}
}
