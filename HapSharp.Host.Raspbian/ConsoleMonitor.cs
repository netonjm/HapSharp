using System;

namespace HapSharp.Host.Raspbian
{
	partial class Program
	{
		class ConsoleMonitor : IMonitor
		{
			public void WriteLine(string message)
			{
				Console.WriteLine(message);
			}
		}
	}
}
