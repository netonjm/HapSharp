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

		[Test ()]
		public async Task GetClient ()
		{
			var client = new HapClient ("My Client Name", "10.67.1.81", 80);
			client.Pair ("123-45-678");
		}
	}
}
