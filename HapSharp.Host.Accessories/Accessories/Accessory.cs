using System;

namespace HapSharp.Accessories
{
    public abstract class Accessory
    {
        public abstract string Template { get; }
        internal virtual string Prefix => "COMPONENT";
        public string Name { get; set; }
        public string PinCode { get; set; } = "031-45-154";
        public string UserName { get; set; } = "11:22:33:44:55:66";
        public string Manufacturer { get; set; } = "Develop Studios";
        public string Model { get; set; } = "v1.0";
        public string SerialNumber { get; set; } = "A12S345KGB";

        public Accessory (string name = null, string username = null)
        {
            if (name != null) {
                Name = name;
            }

            if (username != null) {
                UserName = username;
            }
        }

        protected virtual void Identify () 
        {
            
        }
    }
}
