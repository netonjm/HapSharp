using System;
using System.IO;
using HapSharp;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;
using IoTSharp.Components;

namespace HapSharp_Host_Raspbian
{
	class ConsoleMonitor : IMonitor
	{
		public void WriteLine(string message)
		{
			Console.WriteLine(message);
		}
	}

	class Program
	{
		class Mpd : MpcPlayerAccessory
		{
			public Mpd () : base("Music", "44:66:23:68:81:11")
			{
				Host = "raspi-wc1.local";
			}
		}

		class MotionSensor : IoTMotionSensorAccessory
		{
			public MotionSensor () 
				: base("PIR", "54:36:44:52:61:14")
			{
				Connector = Connectors.GPIO22;
			}
		}

		class RelayLight : IoTLightAccessory
		{
			public RelayLight () 
				: base("Light", "12:12:55:99:53:72")
			{
				Connector = Connectors.GPIO27;
			}
		}

		static string HapNodeJsPath = "/home/pi/HapSharp/HAP-NodeJS";
		static string AllowedHost = "raspi-wc1";

		static void Main(string[] args)
		{
			//This class provides the handling of the output log messages
			var monitor = new ConsoleMonitor();

			//Our HAP session manages our runner, this step only adds our prefered monitor
			var session = new HapSession(monitor) { 
				Sudo = true, 
				AllowedHosts = new string[] { AllowedHost } 
			};

			session.Add<BridgedCoreMessageDelegate>(new BridgedCore("Baño Bridge", "52:72:41:41:33:14"));
		
			session.Add<IoTMotionSensorMessageDelegate> (new MotionSensor ());
			session.Add<MpcPlayerMessageDelegate> (new Mpd ());
			session.Add<IoTLightMessageDelegate> (new RelayLight ());
			session.Start(HapNodeJsPath);

			while (true) {
			}
		}
	}
}
