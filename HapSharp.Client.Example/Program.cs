using System;
using System.Linq;

namespace HapSharp.Client.Example
{
	class MainClass
	{
		public static void Main (string [] args)
		{
			var discoveredClients = HapClient.GetDiscoveredClients().Result;
			foreach (var cl in discoveredClients) {
				Console.WriteLine (cl);
			}

			var discoveredClient = discoveredClients.FirstOrDefault(s => s.IpAddress == "10.67.1.81");
			if (discoveredClient == null || discoveredClient.Services.Count == 0) {
				throw new Exception("client not found or there is no services available");
			}

			var client = discoveredClient.Services[0]
				.ToClient ("My awesome client");

			client.Pair ("031-45-154");

			Console.WriteLine ("Pair successful");
		
		}
	}
}
