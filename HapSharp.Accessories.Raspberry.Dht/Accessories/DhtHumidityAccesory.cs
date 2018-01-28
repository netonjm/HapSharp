namespace HapSharp.Accessories
{
	public abstract class DhtHumidityAccesory : HumidityAccessory
	{
		public abstract int GpioPin { get; }
		public abstract DhtModel DhtModel { get; }
		public virtual int Delay { get; } = DhtService.DefaultDelay;

		public DhtHumidityAccesory(string name = null, string username = null) : base(name, username)
		{
		}
	}
}
