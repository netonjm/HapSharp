using System;
using System.IO;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;

namespace HapSharp.Host.Terminal
{
	partial class MainClass
	{
		class Mpd : MpdPlayerAccessory
		{
			public Mpd() 
				: base("MyMpd", "12:12:55:99:53:72")
			{
				Host = "raspi-salon.local";
			}
		}

		class BroadLink : BroadlinkAccessory
		{
			public BroadLink()
				: base("MyBroadLink", "32:15:54:99:55:76")
			{
				this.Code = "260014025012291215132813151229121512151328131512151315121500034c5112291215122912151328131512151328131512151215131500034c5013291215122912151328131512151229131512151215131500034e5012291215132912151229121513151229121512151315121500034e5112291215122912151328131512151328131512151215131500034e5013281315122912151229121513151229121513151215121500034f5012291215132813151229121512151328131512151315121500034e5013281315122912151328131512151229121513151215131400034f5012291215132813151229121513151229121512151315121500034e5013281315122912151328131512151229121513151215131500034e5012291215132813151229121513151229121512151315121500034f5012291215122912151329121512151328131512151215131500034e5012291315122912151229121513151229121513151215121500034e5112291215122913151229121512151328131512151215131500034e501328131512291215122913151215122912151315121512150003505013291215122912151328131512151229131512151215131500034c5013291215122912151328131512151229131512151215131500034c5013281315122912151328131512151229121513151215131500034c50132813151229121512291315121512291215131512151215000d051500034f";
				this.Hostname = "";
			}
		}

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
			session.Add<BridgedCoreMessageDelegate> (new BridgedCore ("Laptop Net Bridge", "22:32:43:54:45:14"));

			session.Add<BroadLinkMessageDelegate>(new BroadLink());

			//session.Add<MpdPlayerMessageDelegate>(new Mpd ());

			//There are 2 ways of add accessories:
			//Inline generic way: provides automatic instanciation of selected types
			//Don't allow (yet) add more parameters at constructor like base classes
			//session.Add<CustomLightMessageDelegate> (new LightAccessory ("First Light", "AA:21:4D:87:66:78"));
			//session.Add<CustomRegulableLightMessageDelegate> (new RegulableLightAccessory ("Second Light", "AB:12:45:27:55:73"));
			//session.Add<CustomHumidityMessageDelegate> (new HumidityAccessory ("MyHumidity", "A1:32:55:67:53:72"));

			//session.Add<CustomMotionSensorMessageDelegate> (new MotionSensorAccessory ("My MotionSensor", "A1:32:55:67:53:72"));

			//Accessory in external Library 
			//session.Add<NugetHumidityMessageDelegate>(new CustomHumidityAccessory ("NuGet Humidity", "A1:32:55:67:53:72"));




			//Another way to generate is the classic instanciation 
			//var temperatureAccessory = new TemperatureAccessory ("Temperature", "A1:32:45:67:55:73");
			//var temperatureMessageDelegate = new CustomTemperatureMessageDelegate (temperatureAccessory);

			//session.Add (temperatureAccessory, temperatureMessageDelegate);

			//Now we have some accessories we can start the session..
			//Remember follow the Requisites steps in Github readme.
			//https://github.com/netonjm/HapSharp
			session.Start (hapNodeJsPath);

			Console.ReadKey ();
		}
	}
}
