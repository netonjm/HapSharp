using System;
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
			client.Connect (new iCloud.iCloudLoginCredentials (accessory.PhoneUsername, accessory.PhonePassword, false));
			if (!client.IsConnected) {
				WriteLog ("Error: Login was not correct check user and password values");
			}
		}

		public override bool OnGetPower ()
		{
			return true;
		}

		public override void OnChangePower (bool value)
		{
			SendNotification ();
		}

		public void SendNotification ()
		{
			if (client.Devices != null) {
				var device = client.Devices.FirstOrDefault (s => s.DeviceId == accessory.PhoneDeviceId);
				if (device != null) {
					client.PlaySound ("Hey! Where are you?", device);
				} else {
					WriteLog ("Cannot send a notification. Your device was not found. :-(");
					PrintDevices ();
				}
			} else {
				WriteLog ("Error: No devices found.");
			}
		}

		void PrintDevices () 
		{
			WriteLog ($"Listing your devices...");
			foreach (var device in client.Devices) {
				WriteLog ($"{device.DeviceName} ({device.DeviceDisplayName}) {device.DeviceStatus} : {device.DeviceId}");
			}
		}

		public override void Dispose ()
		{
			client.Disconnect ();
			base.Dispose ();
		}
	}
}
