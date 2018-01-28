using IoTSharp.Components;

namespace HapSharp.Accessories
{
	public class IoTLightAccessory : LightAccessory
	{
		public Connectors Connector { get; set; }

		public bool InverseSwitch { get; set; }

		public IoTLightAccessory(string name = null, string username = null) : base(name, username)
		{
		}
	}
}
