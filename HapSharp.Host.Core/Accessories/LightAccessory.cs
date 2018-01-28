namespace HapSharp.Accessories
{
	public class LightAccessory : Accessory
	{
		public LightAccessory (string name = null, string username = null) : base (name, username)
		{
			
		}

		public override void OnTemplateSet()
		{
			Template = "Light_accessory.js";
		}
	}
}
