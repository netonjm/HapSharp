using System;
using HapSharp.Accessories;
using HapSharp.Accessories.Mpc;

namespace HapSharp.MessageDelegates
{
	public class MpcPlayerMessageDelegate : RegulableLightMessageDelegate
	{
		int port;
		string host;

		bool enabled;
		MpcPlayer client;
		MessageDelegateMonitor logger;

		public override void OnInitialize ()
		{
			logger = new MessageDelegateMonitor (this);
			client = new MpcPlayer (logger);
			client.Initialize ();
		}

		public MpcPlayerMessageDelegate (MpcPlayerAccessory accessory)
			: base (accessory)
		{
			host = accessory.Host;
			port = accessory.Port;
		}

		public override void OnChangePower (bool value)
		{
			if (value) {
				client.Play ();
			} else {
				client.Pause ();
			}
			logger.WriteLine ($"[Set] [{value}]");
		}

		public override bool OnGetPower ()
		{
			client.RefreshStatus ();
			return client.CurrentSong != null;
		}

		public override int OnGetBrightness ()
		{
			return client.GetVolume ();
		}

		public override void OnChangeBrightness (int value)
		{
			client.SetVolume (value);
			WriteLog ($"[Volume] [{value}]");
		}
	}
}
