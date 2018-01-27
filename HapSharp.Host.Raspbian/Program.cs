using System.IO;
using HapSharp;
using HapSharp.Accessories;
using HapSharp.MessageDelegates;
using IoTSharp.Components;

namespace HapSharp_Host_Raspbian
{
	partial class Program
	{
		static void Main(string[] args)
		{
			var hapNodeJsPath = "/home/pi/HapSharp/HAP-NodeJS";

			//This class provides the handling of the output log messages
			var monitor = new ConsoleMonitor();

			//Our HAP session manages our runner, this step only adds our prefered monitor
			var session = new HapSession(monitor) { Sudo = true };

			//Now we need add Accessories and MessagesDelegates
			//Our first element must be a bridgeCore, it contains all other accesories in session
			session.Add<BridgedCore, BridgedCoreMessageDelegate>("Xamarin Net Bridge", "22:32:43:54:65:14");
			session.Add<MotionSensorAccessory, IoTMotionSensorMessageDelegate>("IoT Net Sensor", "24:36:44:52:61:14");
			session.Add<LightAccessory, IoTLightMessageDelegate>("IoT Light", "24:56:43:62:81:17");
			session.Add<MusicAccessory, CustomMusicMessageDelegate>("IoT Music", "44:66:23:68:81:11");

			session.Start(hapNodeJsPath);

			while (true) {
				
			}
		}
	}
}
