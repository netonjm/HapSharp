namespace HapSharp.Accessories
{
    public class BridgedCore : Accessory
    {
        internal virtual int Port => ResourcesService.ServicePort;

        public override string Template => "BridgedCore.js";

        public BridgedCore (string name = null, string username = null) : base (name, username)
        {
            
        }

        public override string OnReplaceTemplate (string template)
        {
            return base.OnReplaceTemplate (template)
                       .Replace (GetTemplateTagId (nameof (Port)), Port.ToString ());
        }
    }
}

