using System;
namespace HapSharp.Core
{
	public class Bridge : Accessory
	{
		readonly public Guid SerialNumber;

		public Bridge (string displayName, Guid serialNumber) : base (displayName, serialNumber)
		{
			_isBridge = true;
		}
	}
}
