
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class MotionSensorMessageDelegate : GetMessageDelegate<bool>
	{
		public MotionSensorMessageDelegate (MotionSensorAccessory accessory) : base (accessory)
		{
		}
	}
}
