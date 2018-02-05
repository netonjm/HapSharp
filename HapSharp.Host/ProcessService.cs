using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace HapSharp
{
	static class ProcessService
	{
		static List<System.Diagnostics.Process> GetProcessesByName (string name)
		{
			var dev = new List<Process> ();
			var processes = Process.GetProcesses ();
			foreach (var item in processes) {
				try {
					if (item.ProcessName.Contains (name)) {
						dev.Add (item);
					}
				} catch (Exception) {
				}
			}
			return dev;
		}

		//HACK: Ugly hack to find the correct process to kill because a bug in mono getting process information in mac
		//https://github.com/mono/mono/issues/6663
		static Process GetNodeProcess ()
		{
			return GetProcessesByName ("node").FirstOrDefault (s => s.MainModule?.FileName == "/usr/local/bin/node");
		}

		//HACK: Ugly hack to find the correct process to kill because a bug in mono getting process information in mac
		//https://github.com/mono/mono/issues/6663
		static Process GetBrokerProcess ()
		{
			//var eee = System.Diagnostics.Process.GetProcesses ().FirstOrDefault (s => s.Id == 45457);
			var all = GetProcessesByName ("mono");
			return all.FirstOrDefault (s => s.ProcessName == "mono-sgen64" && s.StartInfo.Environment["PWD"] == Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location));
		}

		static void TryKillProcess (Func<Process> processFunc)
		{
			//hack for mac
			Process process;
			for (int i = 0;i < 20;i++) {
				process = processFunc ();

				if (process == null) {
					return;
				} else {
					process.Kill ();
				}
				Thread.Sleep (500);
			}
				throw new Exception ("cannot kill the process!!!");
		}

		public static void CleanProcessesInMemory () 
		{
			TryKillProcess (GetNodeProcess);
			//TryKillProcess (GetBrokerProcess);
		}
	}
}
