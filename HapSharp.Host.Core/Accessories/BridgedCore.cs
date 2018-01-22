namespace HapSharp.Core.Accessories
{
    public class BridgedCore : Accessory
    {
        internal virtual int Port => ResourcesService.ServicePort;

        public override string Template => "BridgedCore.js";

        public BridgedCore (string name = null, string username = null) : base (name, username)
        {
            
        }
    }
}

