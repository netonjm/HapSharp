using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class FindMyPhoneAccessory : LightAccessory
	{
		public abstract string Username { get; }
		public abstract string Password { get; }
		public abstract string DeviceId { get; }

		public FindMyPhoneAccessory (string name = null, string username = null) : base (name, username)
		{
		}
	}
}
