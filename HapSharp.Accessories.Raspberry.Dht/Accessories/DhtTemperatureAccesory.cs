namespace HapSharp.Accessories
{
	public abstract class DhtTemperatureAccesory : TemperatureAccessory
	{
		public DhtTemperatureAccesory(string name = null, string username = null) : base(name, username)
		{
		}

		public abstract int GpioPin { get; }
		public abstract DhtModel DhtModel { get; }
		public virtual int Delay { get; } = DhtService.DefaultDelay;
	}
}
