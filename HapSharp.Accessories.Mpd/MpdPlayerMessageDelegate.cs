using System;
using Dapplo.MPD.Client;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class MpdPlayerMessageDelegate : RegulableLightMessageDelegate
	{
		int port;
		string host;

		int volume = 100;
		bool enabled;

		CurrentSong song;
		Status status;

		public override void OnInitialize()
		{
			ExectuteMpd(Refresh);
		}

		public MpdPlayerMessageDelegate(MpdPlayerAccessory lightAccessory)
			: base(lightAccessory)
		{
			host = lightAccessory.Host;
			port = lightAccessory.Port;
		}

		public void ExectuteMpd (Action<MpdClient> handler) 
		{
			try
			{
				var client = MpdClient.CreateAsync(host, port).Result;
				handler(client);
				client.Dispose();
			}
			catch (Exception ex)
			{
				WriteLog(ex.ToString());
			}
		}

		protected override void OnChangePower(bool value)
		{
			ExectuteMpd((client) => client.PauseAsync(!value).Wait());

			WriteLog ($"[Net][{Accessory.Name}][Set] [{value}]");
		}

		public void Refresh (MpdClient client) 
		{
			RefreshCurrentStatusDetails(client);
			if (enabled) {
				RefreshCurrentSongDetails(client);
			}
		}

		protected override bool OnGetPower()
		{
			ExectuteMpd(Refresh);
			return enabled;
		}

		protected override int OnGetBrightness()
		{
			return volume;
		}

		protected override void OnChangeBrightness(int value)
		{
			WriteLog ($"[Net][{Accessory.Name}][Volume] [{value}]");
			ExectuteMpd((client) => client.VolumeAsync(value).Wait());
			volume = value;
		}

		void RefreshCurrentSongDetails(MpdClient client)
		{
			song = client.CurrentSongAsync().Result;
			WriteLog("Current Song Details:");
			WriteLog($"Id: {song.Id.ToString()}");
			WriteLog($"Title: {song.Title}");
			WriteLog($"Name: {song.Name}");
			WriteLog($"File: {song.File}");
			WriteLog($"Position: {song.Pos.ToString()}");
		}

		void RefreshCurrentStatusDetails (MpdClient client)
		{
			status = client.StatusAsync().Result;

			volume = status.Volume;
			enabled = status.PlayState == PlayStates.Playing;
			WriteLog("Player Status:");
			WriteLog($"State: {status.PlayState.ToString()}");
			WriteLog($"Volume: {status.Volume.ToString()}");
			WriteLog($"Time: {status.Elapsed.TotalMinutes}/{status.Duration.TotalMinutes}");
		}
	}
}
