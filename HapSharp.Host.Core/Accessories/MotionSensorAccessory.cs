
namespace HapSharp.Accessories
{
	public class MotionSensorAccessory : Accessory
	{
		public bool Value { get; set; } = false;

		public MotionSensorAccessory(string name = null, string username = null) : base(name, username)
		{
			Interval = 500;
		}

		public override void OnTemplateSet()
		{
			Template = "MotionSensor_accessory.js";
		}
	}
}
