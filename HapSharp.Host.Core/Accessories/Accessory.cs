using System;

namespace HapSharp.Core.Accessories
{
    public abstract class Accessory
    {
        public string Id { get; private set; }
        public abstract string Template { get; }
        internal virtual string Prefix => "COMPONENT";
        public string Name { get; private set; }
        public string PinCode { get; set; } = "031-45-154";
        public string UserName { get; private set; } = "11:22:33:44:55:66";
        public string Manufacturer { get; set; } = "Develop Studios";
        public string Model { get; set; } = "v1.0";
        public string SerialNumber { get; set; } = "A12S345KGB";

        public Accessory (string name, string username)
        {
            Name = name;
            UserName = username;

            Id = name.Replace (" ", "") + username.Replace (":", "").ToLower ();
        }

        protected virtual void Identify () 
        {
            
        }
    }
}
