namespace HapSharp.Core.Accessories
{
    public class TemperatureAccessory : Accessory
    {
        public int Temperature { get; set; } = 30;
        public int Interval { get; set; } = 6000;
        public override string Template => "Temperature_accessory.js";

        public TemperatureAccessory(string name = null, string username = null) : base (name, username)
        {

        }
    }
}
