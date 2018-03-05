using System;
using System.IO;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;

namespace HapSharp.Host.Terminal
{
	partial class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine("You need add the path of your HAP-NodeJS directory to continue like an argument.");
				Console.WriteLine("Ex: mono HapSharp.Host.Console.exe /Users/user/HapSharp/HAP-NodeJS");
				return;
			}

			var hapNodeJsPath = args[0];

			if (!Directory.Exists(hapNodeJsPath))
			{
				throw new DirectoryNotFoundException(hapNodeJsPath);
			}

			//This class provides the handling of the output log messages
			var monitor = new ConsoleMonitor();

			//Our HAP session manages our runner, this step only adds our prefered monitor
			var session = new HapSession (monitor);

			//Now we need add Accessories and MessagesDelegates
			//Our first element must be a bridgeCore, it contains all other accesories in session
			session.Add<BridgedCoreMessageDelegate> (new BridgedCore ("Console Net Bridge", "22:32:43:54:45:14"));

			//Now, we add all the accessories from the Shared Project
			//LightMessageDelegate handles the logic of a simple light with two states on/off
			session.Add<CustomLightMessageDelegate>(new LightAccessory ("Humidity Example", "A1:32:45:66:57:73"));

			//There are different ways of add new accessories, it depends in your needs how to do it
			var motionSensorAccessory = new MotionSensorAccessory ("Motion Sensor Example", "A1:42:35:67:55:73");
			var motionSensorMessageDelegate = new CustomMotionSensorMessageDelegate (motionSensorAccessory);
			session.Add (new AccessoryHost (motionSensorAccessory, motionSensorMessageDelegate));

			var regulableLightAccessory = new RegulableLightAccessory ("Regulable Light Example", "A1:A2:35:A7:15:73");
			session.Add (regulableLightAccessory, new CustomRegulableLightMessageDelegate (regulableLightAccessory));

			session.Add<CustomTemperatureMessageDelegate>(new TemperatureAccessory ("Temperature Example", "11:32:75:36:17:73"));

			//Also you can create custom libraries or nugets to share with the community!
			session.Add<NugetHumidityMessageDelegate>(new CustomHumidityAccessory("Example of Humidity", "A2:12:41:67:55:73"));

			//Now with all accessories added to our host we can start the session

			//¡ Before run next step you will need a Broker running in background !

			//Remember follow the Requisites steps in Github readme!!
			//https://github.com/netonjm/HapSharp

			session.Start (hapNodeJsPath);

			Console.ReadKey ();
		}
	}
}
