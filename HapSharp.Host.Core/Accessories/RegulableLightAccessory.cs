namespace HapSharp.Accessories
{
	public class RegulableLightAccessory : LightAccessory
	{
		public RegulableLightAccessory (string name = null, string username = null) : base (name, username)
		{
			
		}

		public override void OnTemplateSet()
		{
			Template = "LightBulb_accessory.js";
		}
	}
}
