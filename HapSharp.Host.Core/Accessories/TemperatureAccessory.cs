namespace HapSharp.Accessories
{
	public class TemperatureAccessory : Accessory
	{
		public int Temperature { get; set; } = 30;
		public override int Interval => 6000;
		public override string Template => "Temperature_accessory.js";

		public TemperatureAccessory (string name = null, string username = null) : base (name, username)
		{

		}
	}
}
