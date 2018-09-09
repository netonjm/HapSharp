using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zeroconf;

namespace HapSharp.Client.Tests
{
	[TestFixture ()]
	public class ClientTest
	{
		[Test ()]
		public async Task GetDiscoveredClients ()
		{
			var clients = await HapClient.GetDiscoveredClients ();
			foreach (var client in clients) {
				Console.WriteLine (client);
			}
			Assert.AreNotEqual (0, clients.Count);
		}

		[Test ()]
		public void PairClient ()
		{
			var client = new HapClient ("My Client Name", "10.67.1.39", 8080);
			client.Pair ("684-65-986");
		}
	}
}
