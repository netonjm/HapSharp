using System;
using uPLibrary.Networking.M2Mqtt;

namespace HapSharp.Host.Broker
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("[Broker] Starting in localhost...");
			// create and start broker
			MqttBroker broker = new MqttBroker ();
			broker.ClientConnected += (s) => {
				Console.WriteLine ($"[Broker] Client connected: {s.ClientId}");
			};

			broker.ClientDisconnected += (s) => {
				Console.WriteLine ($"[Broker] Client disconnected: {s.ClientId}");
			};
			broker.Start ();
			//Once the broker is started, you applciaiton is free to do whatever it wants. 
			Console.WriteLine ("[Broker] Started. Awaiting clients");
			while (true) {

			}
		}
	}
}
