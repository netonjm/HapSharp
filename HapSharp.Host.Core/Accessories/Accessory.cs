using System;
using HapSharp.MessageDelegates;

namespace HapSharp.Accessories
{
	public static class TemplateHelper
	{
		public static string GetTemplateTagId (string prefix, string name)
		{
			return $"{{{{{prefix}_{name.ToUpper ()}}}}}";
		}
	}

	public abstract class Accessory
	{
		public string Id { get; private set; }
		public string Template { get; protected set; }

		public int Interval { get; set; } = 0;
		public string Name { get; private set; }
		public string PinCode { get; set; } = "031-45-154";
		public string UserName { get; private set; } = "11:22:33:44:55:66";
		public string Manufacturer { get; set; } = "HapSharp";
		public string Model { get; set; } = "v1.0";
		public string SerialNumber { get; set; } = "A12S345KGB";

		public virtual void OnTemplateSet ()
		{
			
		}

		public Accessory (string name, string username)
		{
			Name = name;
			UserName = username;

			Id = name.Replace (" ", "") + username.Replace (":", "").ToLower ();
		}

		public virtual string OnReplaceTemplate (string prefix, string template)
		{
			if (Interval > 0) {
				template = template.Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (Interval)), Interval.ToString ());
			}
			//TODO: we need a strong replace way
			return template.Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (Name)), Name)
						   .Replace (TemplateHelper.GetTemplateTagId (prefix,nameof (PinCode)), PinCode)
						   .Replace (TemplateHelper.GetTemplateTagId (prefix,nameof (UserName)), UserName)
						   .Replace (TemplateHelper.GetTemplateTagId (prefix,nameof (Manufacturer)), Manufacturer)
						   .Replace (TemplateHelper.GetTemplateTagId (prefix,nameof (SerialNumber)), SerialNumber)
						   .Replace (TemplateHelper.GetTemplateTagId (prefix,nameof (Model)), Model);
		}
	}
}
