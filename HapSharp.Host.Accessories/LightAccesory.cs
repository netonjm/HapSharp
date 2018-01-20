using System;

namespace HapNet.Accessories
{
    public class Accessory
    {
        public virtual string Name { get; set; }
        public virtual string PinCode { get; set; } = "031-45-154";
        public virtual string Id { get; set; } = "11:22:33:44:55:66";
        public virtual string Manufacturer { get; set; }
        public virtual string Model { get; set; }
        public virtual string SerialNumber { get; set; }

        protected virtual void Identify () 
        {
            
        }
    }

    public class LightAccessory : Accessory
    {
       

    }
}
