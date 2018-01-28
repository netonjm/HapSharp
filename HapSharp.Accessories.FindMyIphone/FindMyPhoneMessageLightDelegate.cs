using System;
using HapSharp.Accessories;
using iCloudLib;
using System.Linq;

namespace HapSharp.MessageDelegates
{
	public class FindMyPhoneMessageDelegate : LightMessageDelegate 
	{
		public iCloud client;
		readonly FindMyPhoneAccessory accessory;

		public FindMyPhoneMessageDelegate(FindMyPhoneAccessory accessory) : base(accessory)
		{
			this.accessory = accessory;
			client = new iCloud ();
			client.Connect (new iCloud.iCloudLoginCredentials (accessory.Username, accessory.Password, false));
		}

		protected override bool OnGetPower ()
		{
			return true;
		}

		protected override void OnChangePower (bool value)
		{
			SendNotification ();
		}

		void SendNotification ()
		{
			
			if (client.Devices != null) {
				PrintDevices ();
				var device = client.Devices.FirstOrDefault (s => s.DeviceId == accessory.DeviceId);
				if (device != null) {
					client.PlaySound ("Hey! Where are you?", device);
				} else {
					Console.WriteLine ("[FindMyPhone] Cannot send a notification. Your device was not found. :-(");
				}
			}
		}

		void PrintDevices () 
		{
			Console.WriteLine ($"[FindMyPhone] Listing your devices...");
			foreach (var device in client.Devices) {
				Console.WriteLine ($"[FindMyPhone] {device.DeviceName} ({device.DeviceDisplayName}) {device.DeviceStatus} : {device.DeviceId}");
			}
		}

		public override void Dispose ()
		{
			client.Disconnect ();
			base.Dispose ();
		}
	}
}
