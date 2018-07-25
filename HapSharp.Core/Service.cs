/*
* Service represents a set of grouped values necessary to provide a logical function. For instance, a
* "Door Lock Mechanism" service might contain two values, one for the "desired lock state" and one for the
* "current lock state". A particular Service is distinguished from others by its "type", which is a UUID.
* HomeKit provides a set of known Service UUIDs defined in HomeKitTypes.js along with a corresponding
* concrete subclass that you can instantiate directly to setup the necessary values. These natively-supported
* Services are expected to contain a particular set of Characteristics.
*
* Unlike Characteristics, where you cannot have two Characteristics with the same UUID in the same Service,
* you can actually have multiple Services with the same UUID in a single Accessory. For instance, imagine
* a Garage Door Opener with both a "security light" and a "backlight" for the display. Each light could be
* a "Lightbulb" Service with the same UUID. To account for this situation, we define an extra "subtype"
* property on Service, that can be a string or other string-convertible object that uniquely identifies the
* Service among its peers in an Accessory. For instance, you might have `service1.subtype = 'security_light'`
* for one and `service2.subtype = 'backlight'` for the other.
*
* You can also define custom Services by providing your own UUID for the type that you generate yourself.
* Custom Services can contain an arbitrary set of Characteristics, but Siri will likely not be able to
* work with these.
*
* @event 'characteristic-change' => function({characteristic, oldValue, newValue, context}) { }
*        Emitted after a change in the value of one of our Characteristics has occurred.
*/

using System;
using System.Collections.Generic;

namespace HapSharp.Core
{
	public class Service
	{
		public event EventHandler Set;

		List<Service> Characteristics = new List<Service> ();

		public event EventHandler<MessageType> MessageReceived;

		public Service (string displayName, string UUID, string subtype) {
			
		}

		public void Add ()
		{

		}

		public void SetCharacteristic ()
		{

		}

		public Service GetCharacteristic (ServiceCharacteristicType characteristic)
		{
			throw new NotImplementedException ();
		}
	}
}
