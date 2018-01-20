using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace HapSharp
{
    internal class HapSession
    {
        public string Host { get; private set; }
        MqttClient client;

        MessageDelegate messageDelegate = null;

        public bool IsConnected => client.IsConnected;

        public HapSession ()
        {
        }

        public void Start ()
        {
            
        }

        public void Connect (string host)
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
            Console.WriteLine("Connecting to: " + host + " with clientId: " + clientId);
            client.Connect(clientId);
            Console.WriteLine("Connected: " + client.IsConnected);
        }

        public void Add (MessageDelegate messageDelegate)
        {
            this.messageDelegate = messageDelegate;

            //Subscribimos a los topics de cada uno de los accesorios
            Subscribe("/home/light");
        }

        void Subscribe (string topic)
        {
            //var topic = "/home/temperature";
            Console.WriteLine("Suscribed to: " + topic);
            // subscribe to the topic "/home/temperature" with QoS 2 
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        void client_MqttMsgPublishReceived (object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            messageDelegate?.OnMessageReceived(e.Topic, e.Message);
            messageDelegate?.OnMessageReceived(e.Topic, System.Text.Encoding.Default.GetString(e.Message));
        }
    }
}
