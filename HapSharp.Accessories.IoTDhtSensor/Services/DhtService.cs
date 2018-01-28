using System.Runtime.InteropServices;
using System;

namespace HapSharp.Accessories
{
	public class DhtService
	{
		[DllImport("Raspberry_Pi_2_Driver.so")]
		static extern int pi_2_dht_read(int sensor, int pin, out float humidity, out float temperature);
		static object obj = new object();

		public const int DefaultDelay = 5000;

		System.Threading.Timer t;

		int Sensor; //DHT11
		int GpioPin;

		public int Humidity { get; private set; }
		public int Temperature { get; private set; }

		public bool IsRunning { get; private set; }


		public DhtService () 
		{
		}

		public void Start (int gpioPin, DhtModel model, int delay = DefaultDelay) 
		{
			if (IsRunning) {
				return;
			}

			IsRunning = true;
			Sensor = (int)model;
			GpioPin = gpioPin;
			t = new System.Threading.Timer(o => Refresh(), null, 0, delay);
		}

		void Refresh ()
		{
			lock (obj)
			{
				try
				{
					float hum, temp;
					pi_2_dht_read(Sensor, GpioPin, out hum, out temp);

					Humidity = (int)hum;
					Temperature = (int)temp;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
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
