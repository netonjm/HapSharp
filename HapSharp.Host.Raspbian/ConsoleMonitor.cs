using System;
using HapSharp;

namespace HapSharp_Host_Raspbian
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
