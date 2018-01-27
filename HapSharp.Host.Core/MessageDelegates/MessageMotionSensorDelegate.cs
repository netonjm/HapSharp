
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class MessageMotionSensorDelegate : GetMessageDelegate<bool>
	{
		public MessageMotionSensorDelegate (MotionSensorAccessory accessory) : base (accessory)
		{
		}
	}
}
