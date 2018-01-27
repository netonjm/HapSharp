
namespace HapSharp.Accessories
{
	public class MotionSensorAccessory : Accessory
	{
		public bool Value { get; set; } = false;
		public override int Interval => 500;
		public override string Template => "MotionSensor_accessory.js";

		public MotionSensorAccessory(string name = null, string username = null) : base(name, username)
		{

		}
	}
}
