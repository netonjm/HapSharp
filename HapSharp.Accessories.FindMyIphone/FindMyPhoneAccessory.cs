using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class FindMyPhoneAccessory : LightAccessory
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string DeviceId { get; set; }

		public FindMyPhoneAccessory (string name = null, string username = null) : base (name, username)
		{
		}
	}
}
