using System.Runtime.InteropServices;
using System;
using System.Threading.Tasks;

namespace HapSharp.Accessories
{
	public class DhtService : IDisposable
	{
		[DllImport("Raspberry_Pi_2_Driver.so")]
		static extern int pi_2_dht_read(int sensor, int pin, out float humidity, out float temperature);
		static object obj = new object();

		bool finished = false;
		public const int DefaultDelay = 5000;

		int Sensor; //DHT11
		int GpioPin;

		public int Humidity { get; private set; }
		public int Temperature { get; private set; }

		public bool IsRunning { get; private set; }

		public DhtService () 
		{
		}

		Task Initialization { get; set; }

		public void Start (int gpioPin, DhtModel model, int delay = DefaultDelay) 
		{
			lock (obj) {
				if (IsRunning) {
					return;
				}

				IsRunning = true;
				Sensor = (int)model;
				GpioPin = gpioPin;

				Initialization = InitializeAsync(); 	
			}
		}

		async Task InitializeAsync()
		{
			while (!finished)
			{
				try
				{
					float hum, temp;
					pi_2_dht_read(Sensor, GpioPin, out hum, out temp);

					if (hum != 0) {
						Humidity = (int)hum;
					}

					if (temp != 0) {
						Temperature = (int)temp;
					}
				}
				catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
				// Asynchronously initialize this instance.
				await Task.Delay(DefaultDelay);
			}
		}

		public void Cancel ()
		{
			finished = true;
		}

		public void Dispose()
		{
			Cancel();
		}

		static DhtService current;
		public static DhtService Current 
		{
			get {
				if (current == null) {
					current = new DhtService();
				}
				return current;
			}
		}
	}
}
