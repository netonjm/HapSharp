using System;
using System.Diagnostics.Terminal;
using System.Threading.Tasks;

namespace HapSharp.Accessories.Mpc
{
	public enum MpcPlayerState
	{
		Playing, Paused, Stop
	}

	/// <summary>
	/// Process wrapper of Mpc Player version 0.28
	/// </summary>
	class MpcPlayer : IDisposable
	{
		readonly ContinuousTerminalProcess process;
		const string DefaultHost = "localhost";
		public string Host { get; set; } = DefaultHost;
		IMonitor monitor;

		public string CurrentSong { get; private set; }
		public bool Repeat { get; private set; }
		public bool Random { get; private set; }
		public bool Single { get; private set; }
		public bool Consume { get; private set; }

		public int Volume { get; private set; }

		public bool Debug { get; set; }

		public MpcPlayerState CurrentState { get; set; }

		public MpcPlayer (IMonitor monitor)
		{
			this.monitor = monitor;
			process = new ContinuousTerminalProcess ();
			process.ErrorDataReceived += (sender, e) => {
				if (Debug)
					monitor.WriteLine ("Error: " + e.Data);
			};
			process.OutputDataReceived += (sender, e) => {
				if (Debug)
					monitor.WriteLine ("Data: " + e.Data);
			};
		}

		public void Initialize()
		{
			process.Start();
		}

		string ExtractValue (string line, string parameter, string endChar)
		{
			line = line.Substring (line.IndexOf (parameter, StringComparison.OrdinalIgnoreCase))
					   .Substring (parameter.Length)
					   .Trim ();
			var endIndex = line.IndexOf (endChar, StringComparison.OrdinalIgnoreCase);

			return line.Substring (0, endIndex == -1 ? line.Length : endIndex);
		}

		public async Task RefreshStatus ()
		{
			await ExecuteMpcCommand ("status").ConfigureAwait(false);
			var commandLines = process.LastOutputLines;

			CurrentState = MpcPlayerState.Stop;
			CurrentSong = "";

			foreach (var line in commandLines) {
				if (line.StartsWith("[playing]", StringComparison.OrdinalIgnoreCase)) {
					CurrentState = MpcPlayerState.Playing;
				} else if (line.StartsWith("[paused]", StringComparison.OrdinalIgnoreCase)) {
					CurrentState = MpcPlayerState.Paused;
				} else if (line.Contains ("volume:")) {
					Volume = int.Parse (ExtractValue (line, "volume:", "%"));
					Repeat = ExtractValue (line, "repeat:", " ") == "on";
					Random = ExtractValue (line, "random:", " ") == "on";
					Single = ExtractValue (line, "single:", " ") == "on";
					Consume = ExtractValue (line, "consume:", " ") == "on";
				} else {
					CurrentSong = line;
				}
			}
		}

		public string GetMpcExecutableCommand (string command)
		{
			var host = Host == DefaultHost ? "" : $" -h {Host}";
			return $"mpc {host}{command}";
		}

		public async Task ExecuteMpcCommand (string command)
		{
			var executeCommand = GetMpcExecutableCommand (command);
			if (Debug)
				monitor.WriteLine ("Command: " + executeCommand);
			await process.ExecuteCommand (executeCommand, true).ConfigureAwait(false);
		}

		/// <summary>
		/// Start playing at position
		/// </summary>
		public async Task Play (int position = 0)
		{
			CurrentState = MpcPlayerState.Playing;
			await ExecuteMpcCommand ("play" + (position == 0 ? "" : " " + position)).ConfigureAwait(false);
		}

		public async Task<int> GetVolume ()
		{
			await RefreshStatus();
			return Volume;
		}

		/// <summary>
		/// Shuffle the current playlist
		/// </summary>
		public async Task Suffle ()
		{
			await ExecuteMpcCommand ("shuffle").ConfigureAwait(false);
		}

		/// <summary>
		/// Toggle repeat mode, or specify state
		/// </summary>
		/// <returns>The repeat.</returns>
		/// <param name="value">If set to <c>true</c> value.</param>
		public async Task RepeatMode (bool value)
		{
			Repeat = value;
			await ExecuteMpcCommand ("repeat " + (value ? "1" : "0")).ConfigureAwait(false);
		}

		/// <summary>
		/// Toggle random mode, or specify state
		/// </summary>
		/// <returns>The random.</returns>
		/// <param name="value">If set to <c>true</c> value.</param>
		public async Task RandomMode (bool value)
		{
			Random = value;
			await ExecuteMpcCommand ("random " + (value ? "1" : "0")).ConfigureAwait(false);
		}

		/// <summary>
		/// Toggle single mode, or specify state
		/// </summary>
		/// <returns>The single.</returns>
		/// <param name="value">If set to <c>true</c> value.</param>
		public async Task SingleMode (bool value)
		{
			Single = value;
			await ExecuteMpcCommand ("single " + (value ? "1" : "0")).ConfigureAwait(false);
		}

		public async Task SavePlaylist (string file)
		{
			await ExecuteMpcCommand ("save " + file).ConfigureAwait(false);
		}

		/// <summary>
		/// Seeks to the specified position
		/// </summary>
		/// <returns>The seek.</returns>
		/// <param name="percentage">Percentage.</param>
		public async Task Seek (int percentage)
		{
			await ExecuteMpcCommand ("seek " + percentage).ConfigureAwait(false);
		}

		/// <summary>
		/// Scan music directory for updates
		/// </summary>
		/// <returns>The update.</returns>
		/// <param name="path">Path.</param>
		public async Task Update (string path)
		{
			await ExecuteMpcCommand ("update " + path).ConfigureAwait(false);
		}

		/// <summary>
		/// Load file as a playlist
		/// </summary>
		/// <param name="file">File.</param>
		public async Task LoadPlaylist (string file)
		{
			await ExecuteMpcCommand ("load " + file).ConfigureAwait(false);
		}

		/// <summary>
		/// Stop the currently playing playlists
		/// </summary>
		public async Task Stop ()
		{
			CurrentState = MpcPlayerState.Stop;
			await ExecuteMpcCommand ("stop").ConfigureAwait(false);
		}

		/// <summary>
		/// Pauses the currently playing song
		/// </summary>
		public async Task Pause ()
		{
			CurrentState = MpcPlayerState.Paused;
			await ExecuteMpcCommand ("pause").ConfigureAwait(false);
		}

		/// <summary>
		/// Set volume to num or adjusts
		/// </summary>
		/// <param name="volume">Volume.</param>
		public async Task SetVolume (int volume)
		{
			if (volume < 0 || volume > 100)
				return;

			Volume = volume;
			await ExecuteMpcCommand ("volume " + volume).ConfigureAwait(false);
		}

		public void Dispose ()
		{
			process.Dispose ();
		}
	}
}
