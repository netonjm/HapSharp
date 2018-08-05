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
		public async Task<IReadOnlyList<IZeroconfHost>> ProbeForNetworkPrinters ()
		{
			return await
				ZeroconfResolver.ResolveAsync ("_hap._tcp.local.");
		}

		//_hap._tcp

		public async Task EnumerateAllServicesFromAllHosts ()
		{
			ILookup<string, string> domains = await ZeroconfResolver.BrowseDomainsAsync ();
			var responses = await ZeroconfResolver.ResolveAsync (domains.Select (g => g.Key));
			foreach (var resp in responses)
				Console.WriteLine (resp);
		}

		[Test ()]
		public async void TestCase ()
		{
			var lol = await ProbeForNetworkPrinters ();

			//var ini = lol.FirstOrDefault (s => s.DisplayName (s => s.));
			//System.Console.WriteLine ("");

		}
	}
}
