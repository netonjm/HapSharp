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
		const string HapNodeJsPath = "/home/pi/HapSharp/HAP-NodeJS";
		const string AllowedHost = "your-rasberry-hostname";

		static void Main(string[] args)
		{
			//This class provides the handling of the output log messages
			var monitor = new ConsoleMonitor ();

			//Our HAP session manages our runner, this step only adds our prefered monitor
			var session = new HapSession (monitor) {
				Sudo = true, //if we want execute our host with privileges
				AllowedHosts = new string[] { AllowedHost } //ensures your host is the expected, if not an exception raises
			};

			//Now we need add Accessories and MessagesDelegates
			//Our first element must be a bridgeCore, it contains all other accesories in session
			session.Add<BridgedCoreMessageDelegate> (new BridgedCore ("Raspian Net Bridge", "22:32:43:54:45:14"));

			//Now, we add all the accessories from the Shared Project
			//LightMessageDelegate handles the logic of a simple light with two states on/off
			session.Add<CustomLightMessageDelegate> (new LightAccessory ("Humidity Example", "A1:32:45:66:57:73"));

			//There are different ways of add new accessories, it depends in your needs how to do it
			var motionSensorAccessory = new MotionSensorAccessory ("Motion Sensor Example", "A1:42:35:67:55:73");
			var motionSensorMessageDelegate = new CustomMotionSensorMessageDelegate (motionSensorAccessory);
			session.Add (new AccessoryHost (motionSensorAccessory, motionSensorMessageDelegate));

			var regulableLightAccessory = new RegulableLightAccessory ("Regulable Light Example", "A1:A2:35:A7:15:73");
			session.Add (regulableLightAccessory, new CustomRegulableLightMessageDelegate (regulableLightAccessory));

			session.Add<CustomTemperatureMessageDelegate> (new TemperatureAccessory ("Temperature Example", "11:32:75:36:17:73"));

			//Now with all accessories added to our host we can start the session

			//¡ Before run next step you will need a Broker running in background !

			//Remember follow the Requisites steps in Github readme!!
			//https://github.com/netonjm/HapSharp

			session.Start (HapNodeJsPath);

			while (true) {
			}
		}
	}
}
