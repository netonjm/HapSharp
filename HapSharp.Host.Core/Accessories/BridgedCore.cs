namespace HapSharp.Accessories
{
	public class BridgedCore : Accessory
	{
		internal virtual int Port => ResourcesService.ServicePort;

		public override string Template => "BridgedCore.js";

		public BridgedCore (string name = null, string username = null) : base (name, username)
		{

		}

		public override string OnReplaceTemplate (string prefix, string template)
		{
			return base.OnReplaceTemplate (prefix, template)
				       .Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (Port)), Port.ToString ());
		}
	}
}

