using System;
using HapSharp.Accessories;

namespace HapSharp.Host.Terminal
{
    class MainClass
    {
        const string hapNodeJsPath = @"/Users/jmedrano/HapSharp/HAP-NodeJS";

        public static void Main(string[] args)
        {
            var session = new HapSession();
            //Adding Bridged Core
            session.Add(
                new CustomBridgedCoreMessageDelegate(
                    new CustomBridgedCoreAccessory("NetAwesomeBridge", "22:32:43:54:65:14")
                )
            );

            session.Add(
                new CustomLightMessageDelegate (
                    new LightAccessory ("First", "AA:21:4D:87:66:78"), 
                    "/home/light1"
                )
            );

            session.Add(
               new CustomRegulableLightMessageDelegate(
                    new RegulableLightAccessory("Second", "AB:12:45:27:55:73"),
                   "/home/light2"
               )
           );
            
            session.Start (StaticResources.BrokerAddress, hapNodeJsPath);

            Console.WriteLine ("Press a key to exit.");
            Console.ReadKey ();

            session.Stop();
        }
    }
}
