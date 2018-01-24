
namespace HapSharp.Accessories
{
	public class HumidityAccessory : Accessory
	{
		public int Humidity { get; set; } = 30;
		public int Interval { get; set; } = 6000;
		public override string Template => "Humidity_accessory.js";

		public HumidityAccessory (string name = null, string username = null) : base (name, username)
		{

		}

		public override string OnReplaceTemplate (string template)
		{
			return base.OnReplaceTemplate (template)
					   .Replace (GetTemplateTagId (nameof (Interval)), Interval.ToString ());
		}
	}
}
