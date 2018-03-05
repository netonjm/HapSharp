namespace HapSharp.Accessories
{
	public class CustomHumidityAccessory : HumidityAccessory
	{
		public CustomHumidityAccessory(string name = null, string username = null) : base (name, username)
		{

		}

		public override void OnTemplateSet()
		{
			//this property defines a different nodejs template to use with this accessory
			//your resource file must be compiled as EmbeddedResource and resource id must be exact
			Template = "NugetHumidity_accessory.js";
		}
	}
}
