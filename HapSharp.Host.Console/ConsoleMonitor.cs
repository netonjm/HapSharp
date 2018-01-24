using System;

namespace HapSharp.Host.Terminal
{
	partial class MainClass
	{
		class ConsoleMonitor : IMonitor
		{
			public void WriteLine (string message)
			{
				Console.WriteLine (message);
			}
		}
	}
}
