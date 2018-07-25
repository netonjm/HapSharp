using System;
namespace HapSharp.Core
{
	public class Characteristic
	{
		string UUID;
		string displayName;

		public Characteristic (string displayName, string UUID, object props)
		{
			this.displayName = displayName;
			this.UUID = UUID;
			//this.iid = null; // assigned by our containing Service
		}
	}
}
