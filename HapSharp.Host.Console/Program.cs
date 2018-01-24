using System;
using System.IO;
using HapSharp.Accessories;
using HapSharp.Host.Terminal.MessageDelegates;

namespace HapSharp.Host.Terminal
{
    partial class MainClass
    {
        public static void Main(string[] args)
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

            var monitor = new ConsoleMonitor ();

            var session = new HapSession (monitor);

            //Adding Bridged Core
            session.Add(
                new CustomBridgedCoreMessageDelegate(new BridgedCore("Xamarin Net Bridge", "22:32:43:54:65:14")),
                new CustomLightMessageDelegate (new LightAccessory ("First Light", "AA:21:4D:87:66:78")),
                new CustomRegulableLightMessageDelegate (new RegulableLightAccessory ("Second Light", "AB:12:45:27:55:73")),
                new CustomTemperatureMessageDelegate (new TemperatureAccessory ("Temperature", "A1:32:45:67:55:73")),
            );
            
            session.Start (hapNodeJsPath);

            Console.ReadKey ();
        }
    }
}
