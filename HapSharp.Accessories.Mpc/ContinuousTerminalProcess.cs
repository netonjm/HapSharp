//
// ContinuousTerminalProcess.cs
//
// Author:
//       Jose Medrano <jose.medrano@microsoft.com>
//
// Copyright (c) 2014-2016 Microsoft Corp. (http://xamarin.com)
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Linq;

namespace System.Diagnostics.Terminal
{
	/// <summary>
	/// This Terminal progress is a continuous terminal session
	/// It allows to execute multiple commands on demand in the current opened process
	/// and has some features like wait until idle ...
	/// </summary>
	internal class ContinuousTerminalProcess : IDisposable
	{
		static bool IsLinux => IO.Path.DirectorySeparatorChar == '/';

		const string linuxBash = "/bin/bash";
		const string msBash = "cmd.exe";

		public string BashFilePath { get; set; } = IsLinux ? linuxBash : msBash;

		const int waitUntilIdleSleep = 1000;
		const string DefaultPingMessage = "[PING]";
		const int ThresholdMillis = 750;
		bool appIdle;
		int idleCounter;

		public DataReceivedEventHandler ErrorDataReceived;
		public DataReceivedEventHandler OutputDataReceived;
		public EventHandler Started;
		public EventHandler Exited;
		public EventHandler Disposed;

		readonly Process process;

		public bool HasExited => process.HasExited;

		public int ProcessId { get { return process.Id; } }
		public List<string> LastOutputLines { get; private set; } = new List<string> ();
		public string LastOutputLine => LastOutputLines.LastOrDefault ();

		public string WorkingDirectory {
			get { return process.StartInfo.WorkingDirectory; }
			set { process.StartInfo.WorkingDirectory = value; }
		}

		public Dictionary<string, string> EnvironmentVariables {
			get {
				var environtmentVariables = new Dictionary<string, string> ();
				foreach (string item in process.StartInfo.EnvironmentVariables.Keys) {
					environtmentVariables.Add (item, process.StartInfo.EnvironmentVariables[item]);
				}
				return environtmentVariables;
			}
			set {
				if (value != null) {
					foreach (string item in value.Keys) {
						process.StartInfo.EnvironmentVariables[item] = value[item];
					}
				}
			}
		}

		public ContinuousTerminalProcess ()
		{
			process = new Process {
				StartInfo = {
					FileName = BashFilePath,
					RedirectStandardInput = true,
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				},
				EnableRaisingEvents = true
			};
		}

		public void ExecuteCommand (string command, bool waitsUntilIdle = false)
		{
			LastOutputLines.Clear ();
			process.StandardInput.WriteLine (command);

			if (waitsUntilIdle) {
				WaitUntilIdle ();
			}
		}

		void Ping ()
		{
			process.StandardInput.WriteLine ($"echo {DefaultPingMessage}{++idleCounter}");
		}

		/// <summary>
		/// This is a custom implementation of wait iddle cursor is available in a console application (works in mac and windows)
		/// </summary>
		public void WaitUntilIdle ()
		{
			appIdle = false;
			while (!appIdle) {
				Ping ();
				Thread.Sleep (waitUntilIdleSleep);
			}
		}

		public void Start ()
		{
			DataReceivedEventHandler onErrorDataReceived = (o, a) => {
				ErrorDataReceived?.Invoke (o, a);
			};
			DataReceivedEventHandler onOutputDataReceived = (o, a) => {

				if (a.Data.StartsWith (DefaultPingMessage, StringComparison.Ordinal)) {
					int receivedCounter = int.Parse (a.Data.Substring (DefaultPingMessage.Length));
					if (receivedCounter == idleCounter) {
						appIdle = true;
					}
				} else {
					LastOutputLines.Add (a.Data);
					OutputDataReceived?.Invoke (o, a);
				}
			};

			process.Disposed += (s, ea) => {
				process.OutputDataReceived -= onOutputDataReceived;
				process.ErrorDataReceived -= onErrorDataReceived;
			};

			process.OutputDataReceived += onOutputDataReceived;
			process.ErrorDataReceived += onErrorDataReceived;

			EventHandler onExit = null;
			onExit = (o, a) => {
				process.Exited -= onExit;
				process.Dispose ();
			};

			process.Exited += onExit;

			process.Disposed += (s, ea) => {
				Disposed?.Invoke (s, ea);
			};

			bool started = process.Start ();
			if (!started) {
				throw new InvalidOperationException ("Could not start process: " + process);
			}

			process.BeginOutputReadLine ();
			process.BeginErrorReadLine ();

			Started?.Invoke (this, EventArgs.Empty);
		}

		public void Dispose ()
		{
			try {
				process.CancelErrorRead ();
				process.CancelOutputRead ();
				process.Kill ();
				process.Dispose ();
			} catch (Exception) {

			}
		}
	}
}