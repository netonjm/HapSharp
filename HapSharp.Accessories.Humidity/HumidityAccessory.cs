
namespace HapSharp.Accessories
{
	public class HumidityAccessory : Accessory
	{
		public int Humidity { get; set; } = 30;
		public override string Template => "Humidity_accessory.js";
        public override int Interval => 6000;

		public HumidityAccessory (string name = null, string username = null) : base (name, username)
		{

		}
	}
}
