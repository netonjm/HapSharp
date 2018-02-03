using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class FindMyPhoneAccessory : LightAccessory
	{
		public string PhoneUsername { get; set; }
		public string PhonePassword { get; set; }
		public string PhoneDeviceId { get; set; }

		public FindMyPhoneAccessory (string name = null, string username = null) : base (name, username)
		{
		}
	}
}
