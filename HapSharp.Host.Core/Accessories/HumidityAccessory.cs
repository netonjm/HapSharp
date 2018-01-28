
namespace HapSharp.Accessories
{
	public class HumidityAccessory : Accessory
	{
		public int Humidity { get; set; } = 0;

		public HumidityAccessory (string name = null, string username = null) : base (name, username)
		{
			Interval = 6000;
		}

		public override void OnTemplateSet()
		{
			Template = "Humidity_accessory.js";
		}
	}
}
