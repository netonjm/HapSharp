namespace HapSharp.Accessories
{
	public class MpdPlayerAccessory : RegulableLightAccessory
	{
		const string DefaultHost = "localhost";
		const int DefaultPort = 6600;

		public string Host { get; set; } = DefaultHost;
		public int Port { get; set; } = DefaultPort;

		public MpdPlayerAccessory(string name = null, string username = null) : base(name, username)
		{
		}
	}
}
