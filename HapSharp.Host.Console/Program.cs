using System;
using HapSharp.Accessories;
using System.IO;

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
                new CustomBridgedCoreMessageDelegate(
                    new CustomBridgedCoreAccessory("NetAwesomeBridge", "22:32:43:54:65:14")
                )
            );

            session.Add(
                new CustomLightMessageDelegate (
                    new LightAccessory ("First", "AA:21:4D:87:66:78")
                )
            );

            session.Add(
               new CustomRegulableLightMessageDelegate (
                    new RegulableLightAccessory("Second", "AB:12:45:27:55:73")
               )
           );

            session.Add (
                new CustomTemperatureMessageDelegate(
                    new TemperatureAccessory ("Temperature", "A1:32:45:67:55:73")
                )
            );
            
            session.Start (hapNodeJsPath);

            Console.ReadKey ();
        }
    }
}
