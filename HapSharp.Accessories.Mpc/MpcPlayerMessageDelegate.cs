using System;
using System.Threading.Tasks;
using HapSharp.Accessories;
using HapSharp.Accessories.Mpc;

namespace HapSharp.MessageDelegates
{
	public class MpcPlayerMessageDelegate : RegulableLightMessageDelegate
	{
		bool lastValue;
		int port;
		string host;

		MpcPlayer client;
		MessageDelegateMonitor logger;

		public bool Value => client.CurrentState == MpcPlayerState.Playing;

		public int Brightness { get; private set; }

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

		public async override void OnChangePower (bool value)
		{
			if (value) {
				await client.Play().ConfigureAwait(false);
			} else {
				await client.Pause ().ConfigureAwait(false);;
			}
			OnValueChanged();
			logger.WriteLine ($"[Set] [{value}]");
		}

		public override bool OnGetPower ()
		{
			client.RefreshStatus().Wait();

			var newValue = Value;
			if (newValue != lastValue) {
				OnValueChanged();
				lastValue = newValue;
			}
			return newValue;
		}

		public override int OnGetBrightness ()
		{
			Brightness = client.GetVolume().Result;
			return Brightness;
		}

		public override void OnChangeBrightness (int value)
		{
			Brightness = value;
			client.SetVolume(value).Wait();
			WriteLog ($"[Volume] [{value}]");
			OnValueChanged();
		}
	}
}
