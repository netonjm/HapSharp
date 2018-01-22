namespace HapSharp.Core.Accessories
{
    public class LightAccessory : Accessory
    {
        public override string Template => "Light_accessory.js";

        public LightAccessory (string name = null, string username = null) : base (name, username)
        {
            
        }
    }
}
