using System;
using System.Collections.Generic;
using System.IO;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Linq;
using HapSharp.Accessories;

namespace HapSharp
{
    internal class HapSession
    {
        //internal const int Port = 51826;
        List<MessageDelegate> messages = new List<MessageDelegate>();

        string hapNodePath;
        MqttClient client;
        System.Diagnostics.Process proc;
       
        public bool IsConnected => client.IsConnected;
        public string Host { get; private set; }
        public bool Debug { get; internal set; }

        public HapSession ()
        {
            
        }

        public void Start (string host, string hapNodePath)
        {
            this.hapNodePath = hapNodePath;

            Console.WriteLine("---------------");
            Console.WriteLine("|              |");
            Console.WriteLine("|  031-45-154  |");
            Console.WriteLine("|              |");
            Console.WriteLine("---------------");

            ProcessService.KillNode ();

            //This is the process to kill
            ///usr/local/bin/node BridgedCore.js
            //sudo lsof -iTCP:51826 -sTCP:LISTEN

            //eliminamos los ficheros de HAP y los substituimos por nuestros templates
            hapNodePath.RemoveHapNodeJsFiles();

            //regeneramos los ficheros
            string template;
            string filePath;

            foreach (var msg in messages)
            {
                template = msg.GetTemplate()
                              .Replace("{{MQTT_ADDRESS}}", host);

                if (msg is MessageBridgedCoreDelegate) {
                    filePath = Path.Combine(hapNodePath, hapNodePath, ((MessageBridgedCoreDelegate)msg).accessory.Template);
                } else {
                    filePath = Path.Combine(hapNodePath, "accessories", msg.OutputAccessoryFileName);
                }
                File.WriteAllText(filePath, template);
            }

            //STEPS ============

            //Setup
            //1. copy files remotely to raspberry
            //2. git clone or copy to the bin/Debug folder

            //3. check install nodejs (this step is not required
            //4. update node modules with 'npm install'

           
            //ejecutamos nodejs bridged en un proceso 

            //Host
            //1. Host process execution with mono remote debug
                //1.1. Add Messages delegates

                //1.2. Sucribe to topics 
            Connect(host);

            //nos suscribimos a los topics
            foreach (var item in messages)
            {
                Subscribe(item.Topic);
                item.SendMessage += (s, e) =>
                {
                    client.Publish(e.Item1, System.Text.Encoding.Default.GetBytes(e.Item2));
                };
            }

            //1.3. Launch HAP-JS process with all our stuff
            StartHapNodeJs();
            //2. Connect 
        }

      
        internal void Stop()
        {
            try
            {
                proc.Close();
                proc.Kill();
                proc.Dispose();
            }
            catch (Exception)
            {
            }
        }

        void Connect(string host)
        {
            if (client != null)
            {
                client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
                client.Disconnect();
            }

            Host = host;
            client = new MqttClient(host);
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            Console.WriteLine("[Net] Connecting to: " + host + " with clientId: " + clientId);
            client.Connect(clientId);
            Console.WriteLine("[Net] Connected: " + client.IsConnected);
        }

        void StartHapNodeJs () 
        {
            proc = new System.Diagnostics.Process {
                StartInfo = new System.Diagnostics.ProcessStartInfo {
                    FileName = "node",
                    Arguments = "BridgedCore.js",
                    WorkingDirectory = "/Users/jmedrano/HapSharp/HAP-NodeJS",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true, 
                }
            };

            if (Debug) {
                proc.StartInfo.EnvironmentVariables.Add("DEBUG", "*");
            }

            proc.OutputDataReceived += (s, e) => {
                Console.WriteLine("[NodeJS] " + e.Data);
            };

            proc.Start();
            proc.BeginOutputReadLine();
        }

     
        public void Add (params MessageDelegate[] elements)
        {
            foreach (var item in elements) {
                messages.Add(item);
            }
        }

        void Subscribe (string topic)
        {
            Console.WriteLine("[Net] Suscribed to: " + topic);
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        void client_MqttMsgPublishReceived (object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            var msg = messages.FirstOrDefault(s => s.Topic == e.Topic);
            if (msg != null) {
                var message = System.Text.Encoding.Default.GetString(e.Message);
                msg.OnMessageReceived(e.Topic, e.Message);
                msg.OnMessageReceived(e.Topic, message);
                if (message == "identify") {
                    msg.OnIdentify();
                }
            }
        }
    }
}
