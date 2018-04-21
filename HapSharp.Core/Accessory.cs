using System;
using System.Collections.Generic;
using System.Linq;

namespace HapSharp.Core
{
	public enum AccessoryCategory
	{
		OTHER = 1,
		BRIDGE = 2,
		FAN = 3,
		GARAGE_DOOR_OPENER = 4,
		LIGHTBULB = 5,
		DOOR_LOCK = 6,
		OUTLET = 7,
		SWITCH = 8,
		THERMOSTAT = 9,
		SENSOR = 10,
		ALARM_SYSTEM = 11,
		SECURITY_SYSTEM = 11, //Added to conform to HAP naming
		DOOR = 12,
		WINDOW = 13,
		WINDOW_COVERING = 14,
		PROGRAMMABLE_SWITCH = 15,
		RANGE_EXTENDER = 16,
		CAMERA = 17,
		IP_CAMERA = 17, //Added to conform to HAP naming
		VIDEO_DOORBELL = 18,
		AIR_PURIFIER = 19,
		AIR_HEATER = 20, //Not in HAP Spec
		AIR_CONDITIONER = 21, //Not in HAP Spec
		AIR_HUMIDIFIER = 22, //Not in HAP Spec
		AIR_DEHUMIDIFIER = 23, // Not in HAP Spec
		APPLE_TV = 24,
		SPEAKER = 26,
		AIRPORT = 27,
		SPRINKLER = 28,
		FAUCET = 29,
		SHOWER_HEAD = 30
	}

	public class Accessory
	{
		object aid;
		bool _isBridge;
		bool bridged;
		bool reachable;
		bool shouldPurgeUnusedIDs;
	
		public string DisplayName { get; private set; }
		public Guid UUID { get; private set; }

		public ServiceType ServiceType { get; set; }
		public AccessoryCategory Category { get; private set; }

		// If we are a Bridge, these are the Accessories we are bridging
		readonly public List<Accessory> BridgedAccessories = new List<Accessory> (); 
		readonly public List<Service> Services = new List<Service> ();
		internal Dictionary<ServiceCharacteristicType, string> serviceCharacteristic = new Dictionary<ServiceCharacteristicType, string>();

		public virtual void Identify ()
		{

		}

		public Accessory (Service service, string displayName, Guid UUID)
		{
			if (displayName == null)
				throw new Exception ("Accessories must be created with a non-empty displayName.");
			if (UUID == null)
				throw new Exception ("Accessories must be created with a valid UUID.");

			this.DisplayName = displayName;
			this.UUID = UUID;
			this.aid = null; // assigned by us in assignIDs() or by a Bridge
			this._isBridge = false; // true if we are a Bridge (creating a new instance of the Bridge subclass sets this to true)
			this.bridged = false; // true if we are hosted "behind" a Bridge Accessory
			this.reachable = true;
			this.Category = AccessoryCategory.OTHER;
			//this.services = []; // of Service
			//this.cameraSource = null;
			this.shouldPurgeUnusedIDs = true; // Purge unused ids by default

			// create our initial "Accessory Information" Service that all Accessories are expected to have
			serviceCharacteristic.Add (ServiceCharacteristicType.Name, displayName);
			serviceCharacteristic.Add (ServiceCharacteristicType.Manufacturer, "Default-Manufacturer");
			serviceCharacteristic.Add (ServiceCharacteristicType.Model, "Default-Model");
			serviceCharacteristic.Add (ServiceCharacteristicType.SerialNumber, "Default-SerialNumber");
			serviceCharacteristic.Add (ServiceCharacteristicType.FirmwareRevision, "1.0");

			// sign up for when iOS attempts to "set" the Identify characteristic - this means a paired device wishes
		}


		public void OnIdentificationRequest ()
		{
			Console.WriteLine ("Identification request {0}", DisplayName);
			// allow implementors to identify this Accessory in whatever way is appropriate, and pass along
			// the standard callback for completion.
			//this.emit ('identify', paired, callback);
			// debug("[%s] Identification request ignored; no listeners to 'identify' event", this.displayName);
			//callback ();
		}

		public void AddService (Service service)
		{
			//TODO: we need to check UUID, subtype 
			if (Services.Contains (service)) {
				return;
			}

			Services.Add (service);

			if (bridged) {
				_updateConfiguration ();

			} else {
				this.Emit (MessageType.ServiceConfigurationChange, new Tuple<Accessory, Service> (this, service));
			}

			service.MessageReceived += (s, message) => {
				if (message == MessageType.ServiceConfigurationChange) {
					if (!this.bridged) {
						this._updateConfiguration ();
					} else {
						this.Emit (MessageType.ServiceConfigurationChange, new Tuple<Accessory, Service> (this, service));
					}
				} else if (message == MessageType.CharacteristicChange) {
					this.Emit (MessageType.ServiceCharacteristicChange, new Tuple<Accessory, Service> (this, service));

					// if we're not bridged, when we'll want to process this event through our HAPServer
					if (!this.bridged)
						this._handleCharacteristicChange ();
				}
			};

			// listen for changes in characteristics and bubble them up

			service.MessageReceived += (s, message) => {
				if (!this.bridged) {
					this._updateConfiguration ();
				} else {
					this.Emit (MessageType.ServiceConfigurationChange, new Tuple<Accessory, Service> (this, service));
				}
			};
		}

		public void UpdateReachability (bool reachable)
		{
			if (!this.bridged)
				throw new Exception ("Cannot update reachability on non-bridged accessory!");
			this.reachable = reachable;

			Console.WriteLine ("Reachability update is no longer being supported.");
		}

		public void AddBridgedAccessory (Accessory accessory, bool deferUpdate)
		{
			if (accessory._isBridge)
				throw new Exception ("Cannot Bridge another Bridge!");

			if (BridgedAccessories.Any (s => s.UUID == accessory.UUID)) {
				throw new Exception ("Cannot add a bridged Accessory with the same UUID as another bridged Accessory: " + accessory.UUID);
			}
			//accessory.
		}

		private void _handleCharacteristicChange ()
		{
			throw new NotImplementedException ();
		}

		public Service GetService (string name)
		{
			throw new NotImplementedException ();
		}

		public void Emit (MessageType order, object args)
		{
			throw new NotImplementedException ();
		}

		private void _updateConfiguration ()
		{
			throw new NotImplementedException ();
		}
	}
}
