using System;
using System.IO;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;

namespace HapSharp.Host.Terminal
{
	partial class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length != 1) {
				Console.WriteLine ("You need add the path of your HAP-NodeJS directory to continue like an argument.");
				Console.WriteLine ("Ex: mono HapSharp.Host.Console.exe /Users/user/HapSharp/HAP-NodeJS");
				return;
			}

			var hapNodeJsPath = args[0];

			if (!Directory.Exists (hapNodeJsPath)) {
				throw new DirectoryNotFoundException (hapNodeJsPath);
			}

			//This class provides the handling of the output log messages
			var monitor = new ConsoleMonitor ();

			//Our HAP session manages our runner, this step only adds our prefered monitor
			var session = new HapSession (monitor);

			//Now we need add Accessories and MessagesDelegates
			//Our first element must be a bridgeCore, it contains all other accesories in session
			session.Add<BridgedCore, BridgedCoreMessageDelegate> ("Laptop Net Bridge", "22:32:43:54:45:14");

			//There are 2 ways of add accessories:
			//Inline generic way: provides automatic instanciation of selected types
			//Don't allow (yet) add more parameters at constructor like base classes
			session.Add<LightAccessory, CustomLightMessageDelegate> ("First Light", "AA:21:4D:87:66:78");
			session.Add<RegulableLightAccessory, CustomRegulableLightMessageDelegate> ("Second Light", "AB:12:45:27:55:73");
			session.Add<HumidityAccessory, CustomHumidityMessageDelegate> ("MyHumidity", "A1:32:55:67:53:72");

			session.Add<MotionSensorAccessory, CustomMotionSensorMessageDelegate> ("My MotionSensor", "A1:32:55:67:53:72");

			//Accessory in external Library 
			session.Add<CustomHumidityAccessory, NugetHumidityMessageDelegate>("NuGet Humidity", "A1:32:55:67:53:72");

			//Another way to generate is the classic instanciation 
			var temperatureAccessory = new TemperatureAccessory ("Temperature", "A1:32:45:67:55:73");
			var temperatureMessageDelegate = new CustomTemperatureMessageDelegate (temperatureAccessory);

			session.Add (temperatureAccessory, temperatureMessageDelegate);

			//Now we have some accessories we can start the session..
			//Remember follow the Requisites steps in Github readme.
			//https://github.com/netonjm/HapSharp
			session.Start (hapNodeJsPath);

			Console.ReadKey ();
		}
	}
}
