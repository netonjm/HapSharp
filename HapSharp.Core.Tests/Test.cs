using NUnit.Framework;
using System;
namespace HapSharp.Core.Tests
{
	[TestFixture ()]
	public class AccessoryTests
	{
		readonly Service service;

		[Test ()]
		public void Bridge_CheckInitialValues ()
		{
			var bridgeGuid = Guid.NewGuid ();
			var bridge = new Bridge ("bridge", bridgeGuid);
			Assert.IsTrue (bridge._isBridge);
		}

		[Test ()]
		public void Accessory_CheckInitialValues ()
		{
			string accessoryName = "test";
			var accessoryGuid = Guid.NewGuid ();

			var accessory = new Accessory (accessoryName, accessoryGuid);
			Assert.AreEqual (accessoryGuid, accessory.UUID);
			Assert.AreEqual (AccessoryCategory.OTHER, accessory.Category);
			Assert.AreEqual (0, accessory.BridgedAccessories.Count);

			Assert.IsFalse (accessory._isBridge);

			Assert.AreEqual (5, accessory.serviceCharacteristic.Count);
			Assert.IsTrue (accessory.serviceCharacteristic.TryGetValue (ServiceCharacteristicType.Manufacturer, out string manufacturer));
			Assert.AreEqual ("Default-Manufacturer", manufacturer);

			Assert.IsTrue (accessory.serviceCharacteristic.TryGetValue (ServiceCharacteristicType.Name, out string displayName));
			Assert.AreEqual (accessoryName, displayName);
		
			Assert.IsTrue (accessory.serviceCharacteristic.TryGetValue (ServiceCharacteristicType.Model, out string model));
			Assert.AreEqual ("Default-Model", model);

			Assert.IsTrue (accessory.serviceCharacteristic.TryGetValue (ServiceCharacteristicType.SerialNumber, out string serial));
			Assert.AreEqual ("Default-SerialNumber", serial);

			Assert.IsTrue (accessory.serviceCharacteristic.TryGetValue (ServiceCharacteristicType.FirmwareRevision, out string firmwareRevision));
			Assert.AreEqual ("1.0", firmwareRevision);
		}
	}
}
