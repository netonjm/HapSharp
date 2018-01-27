namespace HapSharp.Accessories
{
	public class CustomHumidityAccessory : HumidityAccessory
	{
		public override string Template => "NugetHumidity_accessory.js";

		public CustomHumidityAccessory(string name = null, string username = null) : base (name, username)
		{

		}
	}
}
