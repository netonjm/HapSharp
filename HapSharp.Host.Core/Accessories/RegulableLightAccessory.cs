namespace HapSharp.Core.Accessories
{
    public class RegulableLightAccessory : LightAccessory
    {
        public override string Template => "LightBulb_accessory.js";

        public RegulableLightAccessory(string name = null, string username = null) : base (name, username)
        {

        }
    }
}
