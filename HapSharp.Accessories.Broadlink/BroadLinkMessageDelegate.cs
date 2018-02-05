using Broadlink.NET;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class BroadLinkMessageDelegate : LightMessageDelegate
	{
		BroadLinkService service;
		RMDevice device;

		readonly BroadlinkAccessory accessory;
		public BroadLinkMessageDelegate(BroadlinkAccessory accessory) : base(accessory)
		{
			this.accessory = accessory;
		}

		public override void OnInitialize()
		{
			service = new BroadLinkService();
			device = service.DiscoverDevice()
							.WaitUntilReady();
		}

		public override void OnChangePower(bool value)
		{
			device.SendCommand(accessory.Code);
		}

		public override bool OnGetPower()
		{
			return false;
		}

		public override void Dispose()
		{
			device.Dispose();
			service.Dispose();
		}
	}
}
