namespace HapSharp.Accessories
{
	public class DhtTemperatureAccesory : TemperatureAccessory
	{
		public int GpioPin { get; set; }
		public DhtModel DhtModel { get; set; }
		public int Delay { get; } = DhtService.DefaultDelay;

		public DhtTemperatureAccesory(string name = null, string username = null) : base(name, username)
		{
		}
	}
}
