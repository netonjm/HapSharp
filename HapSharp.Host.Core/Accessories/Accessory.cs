using System;

namespace HapSharp.Accessories
{
    public abstract class Accessory
    {
        public string Id { get; private set; }
        public abstract string Template { get; }
        protected virtual string Prefix => "COMPONENT";
		public virtual int Interval => 0;
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

        public virtual string OnReplaceTemplate (string template)
        {
			if (Interval > 0) {
				template = template.Replace (GetTemplateTagId (nameof (Interval)), Interval.ToString ());
			}
            //TODO: we need a strong replace way
            return template.Replace (GetTemplateTagId (nameof (Name)), Name)
                           .Replace (GetTemplateTagId (nameof (PinCode)), PinCode)
                           .Replace (GetTemplateTagId (nameof (UserName)), UserName)
                           .Replace (GetTemplateTagId (nameof (Manufacturer)), Manufacturer)
                           .Replace (GetTemplateTagId (nameof (SerialNumber)), SerialNumber)
                           .Replace (GetTemplateTagId (nameof (Model)), Model);

        }

        public string GetTemplateTagId (string name)
        {
            return $"{{{{{Prefix}_{name.ToUpper ()}}}}}";
        }
    }
}
