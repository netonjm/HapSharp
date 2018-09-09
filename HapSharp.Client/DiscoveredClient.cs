using System.Collections.Generic;
using Zeroconf;

namespace HapSharp.Client
{
	public class DiscoveredClient
	{
		public List<DiscoveredService> Services = new List<DiscoveredService> ();

		public string IpAddress => host.IPAddress;
		public string Id => host.Id;
		public string DisplayName => host.DisplayName;

		public DiscoveredClient (IZeroconfHost host)
		{
			this.host = host;

			foreach (var service in host.Services) {
				Services.Add (new DiscoveredService (service.Key, service.Value, this));
			}
		}

		readonly IZeroconfHost host;
	}

	public class DiscoveredService
	{
		public string Name { get; private set; }
		public int Port => service.Port;
		public int Ttl => service.Ttl;

		public DiscoveredService(string name, IService service, DiscoveredClient client)
		{
			this.client = client;
			this.service = service;
			Name = name;
		}
		readonly DiscoveredClient client;
		readonly IService service;

		public HapClient ToClient (string name) =>  new HapClient(name, client.IpAddress, service.Port);
	}
}
