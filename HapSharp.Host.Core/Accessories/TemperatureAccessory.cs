namespace HapSharp.Accessories
{
	public class TemperatureAccessory : Accessory
	{
		public int Temperature { get; set; } = 0;

		public TemperatureAccessory (string name = null, string username = null) : base (name, username)
		{
			Interval = 6000;
		}

		public override void OnTemplateSet()
		{
			Template = "Temperature_accessory.js";
		}
	}
}
