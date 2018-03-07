using System;
namespace HapSharp.Core
{
	public enum ServiceType
	{
		AccessoryInformation
	}

	public enum ServiceCharacteristicType
	{
		Name, Manufacturer, Model, SerialNumber, FirmwareRevision, Identify
	}

	public class CharacteristicService
	{
		public event EventHandler Set;

		public void Add ()
		{

		}

		public void SetCharacteristic ()
		{

		}
	}
}
