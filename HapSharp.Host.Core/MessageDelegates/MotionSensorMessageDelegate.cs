
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class MotionSensorMessageDelegate : GetMessageDelegate<bool>
	{
		public abstract bool Value { get; }

		public MotionSensorMessageDelegate (MotionSensorAccessory accessory) : base (accessory)
		{
		}
	}
}
