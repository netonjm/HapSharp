using System;
namespace HapSharp.Core
{
	public interface IUUID
	{
		string Generate (string data);
		bool IsValid (string UUID);
		string Unparse (string bug, int offset);
	}

	public enum EventService
	{
		CharacteristicChange,
		ServiceConfigurationChange,
	}

	public interface IEventEmitterAccessory
	{
		void AddListener (EventAccessory eventAccessory, Action handler);
		void On (EventAccessory eventAccessory, Action handler);
		void Once (EventAccessory eventAccessory, Action handler);
		void RemoveListener (EventAccessory eventAccessory, Action handler);
		void RemoveAllListeners (EventAccessory eventAccessory);
		void SetMaxListeners (int number);
		int GetMaxListeners ();
	}

	public interface IService : IEventEmitterAccessory
	{
		//new (displayName: string, UUID: string, subtype: string): Service;
		string displayName { get; set; }
		string UUID { get; set; }
		string subtype { get; set; }
		string iid { get; set; }

		ICharacteristic[] characteristics { get; set; }
		ICharacteristic[] optionalCharacteristics { get; set; }

		ICharacteristic addCharacteristic (ICharacteristic characteristic, Action handler);

		void removeCharacteristic (ICharacteristic characteristic);

		ICharacteristic getCharacteristic (string characteristic, string value);

		bool testCharacteristic (string characteristic);
		bool setCharacteristic (string characteristic);

		IService updateCharacteristic (string name, string value);
		void addOptionalCharacteristic (ICharacteristic characteristic);
		ICharacteristic getCharacteristicByIID (string iid);

		string toHAP (object any);
	}

	public interface ICharacteristic
	{

	}

	public enum EventAccessory
	{
		ServiceConfigurationChange,
		ServiceCharacteristicChange,
		Identify
	}

	public enum EventCharacteristic
	{
		Get,
		Set,
	}

	public enum ServiceType
	{
		AccessoryInformation
	}

	public enum ServiceCharacteristicType
	{
		Name, Manufacturer, Model, SerialNumber, FirmwareRevision, Identify
	}
}
