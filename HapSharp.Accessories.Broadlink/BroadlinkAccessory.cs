using System;

namespace HapSharp.Accessories
{
	public class BroadlinkAccessory : LightAccessory
	{
		public string Code { get; set; }
		public string Hostname { get; set; }

		public BroadlinkAccessory(string name = null, string username = null) : base(name, username)
		{
		}
	}
}
