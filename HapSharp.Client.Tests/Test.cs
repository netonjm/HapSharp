using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zeroconf;

namespace HapSharp.Client.Tests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public async Task GetActualServices ()
		{
			var services = await HapClient.GetServices ();
			foreach (var service in services) {
				Console.WriteLine (service);
			}
			Assert.AreNotEqual (0, services.Count);
		}
	}
}
