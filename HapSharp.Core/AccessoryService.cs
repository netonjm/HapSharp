using System;
using System.Collections.Generic;

namespace HapSharp.Core
{
	public class AccessoryService
	{
		List<CharacteristicService> Characteristics = new List<CharacteristicService> ();

		public event EventHandler<MessageType> MessageReceived;

		public void Add ()
		{

		}

		public CharacteristicService GetCharacteristic (ServiceCharacteristicType characteristic)
		{
			return new CharacteristicService ();
		}
	}
}
