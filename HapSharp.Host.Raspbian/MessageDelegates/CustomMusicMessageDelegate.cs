using System;
using System.IO;
using System.Reflection;
using HapSharp.Accessories;
using IoTSharp.Components;

namespace HapSharp.MessageDelegates
{
	class CustomMusicMessageDelegate : MessageRegulableLightDelegate
	{
		bool enabled;
		int brightness = 100;
		readonly IoTSoundPlayer soundPlayer;
		readonly string musicPath;

		public CustomMusicMessageDelegate(MusicAccessory musicAccessory)
			: base(musicAccessory)
		{
			musicPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			soundPlayer = new IoTSoundPlayer();
		}

		protected override void OnChangePower(bool value)
		{
			if (value)
			{
				soundPlayer.Play(Path.Combine(musicPath, $"sound1.wav"));
			}
			else
			{
				soundPlayer.Stop();
			}

			enabled = value;
			Console.WriteLine($"[Net][{Accessory.Name}][Set] [{value}]");
		}

		protected override bool OnGetPower()
		{
			return enabled;
		}

		public override void OnIdentify()
		{
			Console.WriteLine($"[Net][{Accessory.Name}] Identified");
		}

		protected override int OnGetBrightness()
		{
			return brightness;
		}

		protected override void OnChangeBrightness(int value)
		{
			enabled = true;
			brightness = value;
			Console.WriteLine($"[Net][{Accessory.Name}][Brightness] [{value}]");
		}
	}
}
