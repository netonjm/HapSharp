namespace HapSharp.Accessories
{
	public class DhtHumidityAccesory : HumidityAccessory
	{
		public int GpioPin { get; set; }
		public DhtModel DhtModel { get; set; }
		public int Delay { get; set; } = DhtService.DefaultDelay;

		public DhtHumidityAccesory(string name = null, string username = null) : base(name, username)
		{
		}
	}
}
