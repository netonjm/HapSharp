using System;
using System.Diagnostics.Terminal;

namespace HapSharp.Accessories.Mpc
{
	/// <summary>
	/// Process wrapper of Mpc Player version 0.28
	/// </summary>
	class MpcPlayer : IDisposable
	{
		readonly ContinuousTerminalProcess process;
		const string DefaultHost = "localhost";
		public string Host { get; set; } = DefaultHost;
		IMonitor monitor;

		public bool Debug { get; set; }

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

		public string CurrentSong { get; private set; }
		public bool Repeat { get; private set; }
		public bool Random { get; private set; }
		public bool Single { get; private set; }
		public bool Consume { get; private set; }

		public int Volume { get; private set; }

		string ExtractValue (string line, string parameter, string endChar)
		{
			line = line.Substring (line.IndexOf (parameter, StringComparison.OrdinalIgnoreCase))
					   .Substring (parameter.Length)
					   .Trim ();
			var endIndex = line.IndexOf (endChar, StringComparison.OrdinalIgnoreCase);

			return line.Substring (0, endIndex == -1 ? line.Length : endIndex);
		}

		public void RefreshStatus ()
		{
			ExecuteMpcCommand ("status");
			var commandLines = process.LastOutputLines;

			CurrentSong = "";

			foreach (var line in commandLines) {
				if (line.StartsWith ("[playing]", StringComparison.OrdinalIgnoreCase)) {

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

		public void Initialize ()
		{
			process.Start ();
		}

		public void ExecuteMpcCommand (string command)
		{
			var executeCommand = GetMpcExecutableCommand (command);
			if (Debug)
				monitor.WriteLine ("Command: " + executeCommand);
			process.ExecuteCommand (executeCommand, true);
		}

		/// <summary>
		/// Start playing at position
		/// </summary>
		public void Play (int position = 0)
		{
			ExecuteMpcCommand ("play" + (position == 0 ? "" : " " + position));
		}

		public int GetVolume ()
		{
			try {
				ExecuteMpcCommand ("volume");
				var volume = process.LastOutputLine;
				volume = volume.Substring (volume.IndexOf (":", StringComparison.Ordinal) + 1);
				volume = volume.Substring (0, volume.IndexOf ("%", StringComparison.Ordinal));
				Volume = int.Parse (volume);
				return Volume;
			} catch (Exception ex) {
				monitor.WriteLine ("Exception: " + ex.Message);
				return -1;
			}
		}

		/// <summary>
		/// Shuffle the current playlist
		/// </summary>
		public void Suffle ()
		{
			ExecuteMpcCommand ("shuffle");
		}

		/// <summary>
		/// Toggle repeat mode, or specify state
		/// </summary>
		/// <returns>The repeat.</returns>
		/// <param name="value">If set to <c>true</c> value.</param>
		public void RepeatMode (bool value)
		{
			ExecuteMpcCommand ("repeat " + (value ? "1" : "0"));
		}

		/// <summary>
		/// Toggle random mode, or specify state
		/// </summary>
		/// <returns>The random.</returns>
		/// <param name="value">If set to <c>true</c> value.</param>
		public void RandomMode (bool value)
		{
			ExecuteMpcCommand ("random " + (value ? "1" : "0"));
		}

		/// <summary>
		/// Toggle single mode, or specify state
		/// </summary>
		/// <returns>The single.</returns>
		/// <param name="value">If set to <c>true</c> value.</param>
		public void SingleMode (bool value)
		{
			ExecuteMpcCommand ("single " + (value ? "1" : "0"));
		}

		public void SavePlaylist (string file)
		{
			ExecuteMpcCommand ("save " + file);
		}

		/// <summary>
		/// Seeks to the specified position
		/// </summary>
		/// <returns>The seek.</returns>
		/// <param name="percentage">Percentage.</param>
		public void Seek (int percentage)
		{
			ExecuteMpcCommand ("seek " + percentage);
		}

		/// <summary>
		/// Scan music directory for updates
		/// </summary>
		/// <returns>The update.</returns>
		/// <param name="path">Path.</param>
		public void Update (string path)
		{
			ExecuteMpcCommand ("update " + path);
		}

		/// <summary>
		/// Load file as a playlist
		/// </summary>
		/// <param name="file">File.</param>
		public void LoadPlaylist (string file)
		{
			ExecuteMpcCommand ("load " + file);
		}

		/// <summary>
		/// Stop the currently playing playlists
		/// </summary>
		public void Stop ()
		{
			ExecuteMpcCommand ("stop");
		}

		/// <summary>
		/// Pauses the currently playing song
		/// </summary>
		public void Pause ()
		{
			ExecuteMpcCommand ("pause");
		}

		/// <summary>
		/// Set volume to num or adjusts
		/// </summary>
		/// <param name="volume">Volume.</param>
		public void SetVolume (int volume)
		{
			if (volume < 0 || volume > 100)
				throw new Exception ("Volume must be in range 0/100");
			ExecuteMpcCommand ("volume " + volume);
		}

		public void Dispose ()
		{
			process.Dispose ();
		}
	}
}
