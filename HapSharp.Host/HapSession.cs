using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using HapSharp.MessageDelegates;
using System.Reflection;
using HapSharp.Accessories;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace HapSharp
{
	public class HapSession : IDisposable
	{
		const string DefaultBrokerHost = "localhost";

		public string[] AllowedHosts { get; set; }

		internal const int Port = 51826;
		readonly List<AccessoryHost> accessoriesHosts = new List<AccessoryHost> ();
		readonly IMonitor monitor;

		Process process;
		MqttClient client;
		string hapNodePath;

		public string BrokerHost { get; private set; }

		public bool Debug { get; internal set; }
		public bool Sudo { get; set; }

		public bool IsConnected => client.IsConnected;

		public HapSession (IMonitor monitor)
		{
			this.monitor = monitor;
		}

		public void CheckHost ()
		{
			//Is our device allowed to execute this host
			if (AllowedHosts != null) {
				if (!AllowedHosts.Contains(Environment.MachineName)) {
					throw new UnauthorizedAccessException("Your device is not allowed to execute this Host session.");
				}
			}
		}

		public void Start(string hapNodePath, string brokerHost = DefaultBrokerHost)
		{
			this.hapNodePath = hapNodePath;

			CheckHost ();

			//Kill current user node processes
			ProcessService.CleanProcessesInMemory ();

			//clean native .js files in HAP-NodeJS folder
			hapNodePath.RemoveHapNodeJsFiles ();

			//accessory initialization
			InitializeAccessories ();

			//re-generate native .js accessories based from our message delegates
			WriteAccessories ();

			//Starts a local broker with logging
			//StartLocalBroker ();

			//Connection to current MQTT broker
			ConnectToBroker (brokerHost);

			//We need subscribe to all topics
			SubscribeAllTopics ();

			//Launches HAP-NodeJS process
			StartHapNodeJs ();

			//Prints on console current PinCode to make easy the user add the bridge
			PrintCurrentCode ();

			monitor.WriteLine ($"[Net] Host started in port: {Port}");
		}

		void SubscribeAllTopics ()
		{
			foreach (var msgDelegate in accessoriesHosts.Select (s => s.MessageDelegate)) {
				Subscribe (msgDelegate.Topic);
				msgDelegate.SendMessage += (s, e) => {
					client.Publish (e.Item1, System.Text.Encoding.Default.GetBytes (e.Item2));
				};
			}
		}

		void InitializeAccessories ()
		{
			monitor.WriteLine($"[Net] Starting accessories initialization...");
			foreach (var accHost in accessoriesHosts) {
				accHost.MessageDelegate.monitor = monitor;
				accHost.MessageDelegate.OnInitialize();
			}
		}

		void WriteAccessories ()
		{
			string filePath;

			foreach (var accHost in accessoriesHosts) {

				accHost.Accessory.OnTemplateSet();

				if (accHost.MessageDelegate is BridgedCoreMessageDelegate) {
					filePath = Path.Combine (hapNodePath, hapNodePath,  accHost.Accessory.Template);
				} else {
					filePath = Path.Combine (hapNodePath, "accessories", accHost.MessageDelegate.OutputAccessoryFileName);
				}

				var template = GetProcessedTemplate (accHost);
				File.WriteAllText (filePath, template);
			}
		}

		string GetProcessedTemplate (AccessoryHost accessoryHost)
		{
			var delegateType = accessoryHost.MessageDelegate.GetType ();

			var template = GetTemplateFromResourceId (delegateType, accessoryHost.Accessory.Template);
			if (template == null) {
				throw new Exception ("Resource was not found in assemblies");
			}
			template = accessoryHost.OnReplaceTemplate (template);
			return template.Replace ("{{MQTT_ADDRESS}}", BrokerHost);
		}

		public Type GetAccessoryFirstType (Type type)
		{
			if (type.BaseType == typeof (MessageDelegate))
				return type;
			return GetAccessoryFirstType (type.BaseType);
		}

		string GetTemplateFromResourceId (Type type, string resourceId)
		{
			var template = ResourcesService.GetManifestResource (type.Assembly, resourceId);
			if (template == null) {
				return template = GetTemplateFromResourceId (type.BaseType, resourceId);
			}
			return template;
		}

		void ConnectToBroker (string host)
		{
			if (client != null) {
				client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
				client.Disconnect ();
			}

			BrokerHost = host;

			client = new MqttClient (host);
			client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

			string clientId = Guid.NewGuid ().ToString ();
			monitor.WriteLine ($"[Net] Connecting to: {host} with clientId: {clientId}");
			client.Connect (clientId);
			monitor.WriteLine ($"[Net] Connected: {client.IsConnected}");
		}

		void StartHapNodeJs ()
		{
			var filename = Sudo ? "sudo" : "node";
			var arguments = Sudo ? "node BridgedCore.js" : "BridgedCore.js";
			process = new Process {
				StartInfo = new ProcessStartInfo {
					FileName = filename,
					Arguments = arguments,
					WorkingDirectory = hapNodePath,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
				}
			};

			if (Debug) {
				process.StartInfo.EnvironmentVariables.Add ("DEBUG", "*");
			}

			process.OutputDataReceived += (s, e) => {
				monitor.WriteLine ($"[NodeJS]{e.Data}");
			};

			process.Start ();
			process.BeginOutputReadLine ();
		}

		public void Add (params AccessoryHost[] elements)
		{
			foreach (var item in elements) {
				accessoriesHosts.Add (item);
			}
		}

		public void Add (Accessory accessory, MessageDelegate messageDelegate)
		{
			accessoriesHosts.Add (new AccessoryHost (accessory, messageDelegate));
		}

		public void Add<T1> (Accessory accessory)
		{
			var message = (MessageDelegate) Activator.CreateInstance (typeof (T1), new object[] { accessory });
			accessoriesHosts.Add (new AccessoryHost (accessory, message));
		}

		void Subscribe (string topic)
		{
			monitor.WriteLine ("[Net] Suscribed to: " + topic);
			client.Subscribe (new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
		}

		void client_MqttMsgPublishReceived (object sender, MqttMsgPublishEventArgs e)
		{
			var msg = accessoriesHosts.FirstOrDefault (s => s.MessageDelegate.Topic == e.Topic).MessageDelegate;
			if (msg != null) {
				var message = System.Text.Encoding.Default.GetString (e.Message);
				if (message == "identify") {
					msg.OnIdentify ();
				} else {
					msg.RaiseMessageReceived (e.Topic, e.Message);
					msg.RaiseMessageReceived (e.Topic, message);
				}
			}
		}

		void PrintCurrentCode ()
		{
			var pinCode = accessoriesHosts
				.FirstOrDefault (s => s.MessageDelegate is BridgedCoreMessageDelegate)
				.Accessory.PinCode;

			monitor.WriteLine ("---------------");
			monitor.WriteLine ("|              |");
			monitor.WriteLine ($"|  {pinCode}  |");
			monitor.WriteLine ("|              |");
			monitor.WriteLine ("---------------");
		}

		internal void Stop ()
		{
			if (client.IsConnected) {
				client.Disconnect ();
			}
			KillProcess (process);
		}

		void KillProcess (Process process)
		{
			try {
				process.Close ();
				process.Kill ();
				process.Dispose ();
			} catch {
			}
		}

		public void Dispose ()
		{
			Stop ();
		}
	}
}
