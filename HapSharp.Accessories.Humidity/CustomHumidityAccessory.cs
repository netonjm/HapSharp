namespace HapSharp.Accessories
{
	public class CustomHumidityAccessory : HumidityAccessory
	{
		public CustomHumidityAccessory(string name = null, string username = null) : base (name, username)
		{

		}

		public override void OnTemplateSet()
		{
			Template = "NugetHumidity_accessory.js";
		}
	}
}
