using System;
using System.Collections.Generic;
using System.IO;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Linq;
using System.Diagnostics;
using HapSharp.Core.MessageDelegates;

namespace HapSharp
{
    public class HapSession : IDisposable
    {
        const string DefaultBrokerHost = "broker.hivemq.com";

        //internal const int Port = 51826;
        readonly List<MessageDelegate> messages = new List<MessageDelegate> ();
        readonly IMonitor monitor;

        MqttClient client;
        Process proc;

        string hapNodePath;

        public string Host { get; private set; }
        public bool Debug { get; internal set; }

        public bool IsConnected => client.IsConnected;

        public HapSession (IMonitor monitor)
        {
            this.monitor = monitor;
        }

        public void Start (string hapNodePath, string host = DefaultBrokerHost)
        {
            this.hapNodePath = hapNodePath;

            //Kill current user node processes
            ProcessService.TryKillCurrentNodeProcess ();

            //clean native .js files in HAP-NodeJS folder
            hapNodePath.RemoveHapNodeJsFiles ();

            //re-generate native .js accessories based from our message delegates
            WriteAccessories (host);

            //Connection to current MQTT broker
            ConnectToBroker (host);

            //We need subscribe to all topics
            SubscribeAllTopics ();

            //Launches HAP-NodeJS process
            StartHapNodeJs ();

            //Prints on console current PinCode to make easy the user add the bridge
            PrintCurrentCode ();
        }

        void SubscribeAllTopics ()
        {
            foreach (var item in messages) {
                Subscribe (item.Topic);
                item.SendMessage += (s, e) => {
                    client.Publish (e.Item1, System.Text.Encoding.Default.GetBytes (e.Item2));
                };
            }
        }

        void WriteAccessories (string host) 
        {
            string template;
            string filePath;

            foreach (var msg in messages) {
                template = msg.GetTemplate ()
                              .Replace ("{{MQTT_ADDRESS}}", host);

                if (msg is MessageBridgedCoreDelegate) {
                    filePath = Path.Combine (hapNodePath, hapNodePath, ((MessageBridgedCoreDelegate)msg).accessory.Template);
                } else {
                    filePath = Path.Combine (hapNodePath, "accessories", msg.OutputAccessoryFileName);
                }
                File.WriteAllText (filePath, template);
            }
        }

        void ConnectToBroker (string host)
        {
            if (client != null) {
                client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
                client.Disconnect ();
            }

            Host = host;

            client = new MqttClient (host);
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid ().ToString ();
            monitor.WriteLine ("[Net] Connecting to: " + host + " with clientId: " + clientId);
            client.Connect (clientId);
            monitor.WriteLine ("[Net] Connected: " + client.IsConnected);
        }

        void StartHapNodeJs ()
        {
            proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "node",
                    Arguments = "BridgedCore.js",
                    WorkingDirectory = hapNodePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }
            };

            if (Debug) {
                proc.StartInfo.EnvironmentVariables.Add ("DEBUG", "*");
            }

            proc.OutputDataReceived += (s, e) => {
                monitor.WriteLine ("[NodeJS] " + e.Data);
            };

            proc.Start ();
            proc.BeginOutputReadLine ();
        }


        public void Add (params MessageDelegate[] elements)
        {
            foreach (var item in elements) {
                messages.Add (item);
            }
        }

        void Subscribe (string topic)
        {
            monitor.WriteLine ("[Net] Suscribed to: " + topic);
            client.Subscribe (new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        void client_MqttMsgPublishReceived (object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            var msg = messages.FirstOrDefault (s => s.Topic == e.Topic);
            if (msg != null) {
                var message = System.Text.Encoding.Default.GetString (e.Message);
                if (message == "identify") {
                    msg.OnIdentify ();
                } else {
                    msg.OnMessageReceived (e.Topic, e.Message);
                    msg.OnMessageReceived (e.Topic, message);
                }
            }
        }

        void PrintCurrentCode ()
        {
            var pinCode = messages
                .FirstOrDefault (s => s is MessageBridgedCoreDelegate)
                .accessory.PinCode;
            
            monitor.WriteLine ("---------------");
            monitor.WriteLine ("|              |");
            monitor.WriteLine ($"|  {pinCode}  |");
            monitor.WriteLine ("|              |");
            monitor.WriteLine ("---------------");
        }

        internal void Stop ()
        {
            try {
                if (client.IsConnected) {
                    client.Disconnect ();
                }

                proc.Close ();
                proc.Kill ();
                proc.Dispose ();
            } catch (Exception) {
            }
        }

        public void Dispose ()
        {
            Stop ();
        }
    }
}
