using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HapSharp
{
	static class ProcessService
	{
		static List<System.Diagnostics.Process> GetProcessesByName (string name)
		{
			var dev = new List<System.Diagnostics.Process> ();
			var processes = System.Diagnostics.Process.GetProcesses ();
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

		public static void TryKillCurrentNodeProcess ()
		{
			//hack for mac
			var process = GetProcessesByName ("node").FirstOrDefault (s => s.MainModule?.FileName == "/usr/local/bin/node");

			if (process != null) {
				var id = process.Id;
				process?.Kill ();

				for (int i = 0;i < 20;i++) {
					process = GetProcessesByName ("node").FirstOrDefault (s => s.MainModule?.FileName == "/usr/local/bin/node");
					if (process == null) {
						return;
					}
					Thread.Sleep (500);
				}
				throw new Exception ("cannot kill the process!!!");
			}
		}
	}
}
