using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class MessageDelegateMonitor : IMonitor
	{
		readonly MessageDelegate msgDelegate;

		public MessageDelegateMonitor (MessageDelegate msgDelegate)
		{
			this.msgDelegate = msgDelegate;
		}

		public void WriteLine (string message)
		{
			msgDelegate.WriteLog (message);
		}
	}

	public abstract class MessageDelegate : IDisposable
	{
		protected const string ReceiveTopicNode = "r";

		public virtual void OnInitialize () 
		{
			
		}

		public event EventHandler<Tuple<string, string>> SendMessage;

		readonly protected Accessory Accessory;

		readonly public string Topic;

		internal string TopicReceive => Topic + "/" + ReceiveTopicNode;

		internal string OutputAccessoryFileName => GetNormalizedFileName (Accessory.Name) + "_accessory.js";

		protected void OnSendMessage (string topic, string message, int value)
		{
			OnSendMessage (topic, message, value.ToString ());
		}

		protected void OnSendMessage(string topic, string message, bool value)
		{
			OnSendMessage(topic, message, value ? "true" : "false");
		}

		protected void OnSendMessage (string topic, string message, string value)
		{
			OnSendMessage (topic + "/" + ReceiveTopicNode, message + "/" + value);
		}

		void OnSendMessage (string topic, string message)
		{
			SendMessage?.Invoke (this, new Tuple<string, string> (topic, message));
		}

		protected MessageDelegate (Accessory accessory)
		{
			this.Accessory = accessory;
			Topic = "home/" + accessory.Id;
		}

		public virtual void OnIdentify ()
		{
			WriteLog ($"[{Accessory.Name}] Identified.");
		}

		string GetNormalizedFileName (string name)
		{
			return name.Replace (" ", "");
		}

		internal void RaiseMessageReceived (string topic, string message)
		{
			OnMessageReceived (topic, message);
		}

		internal void RaiseMessageReceived (string topic, byte[] message)
		{
			OnMessageReceived (topic, message);
		}

		protected virtual void OnMessageReceived (string topic, string message)
		{

		}

		public virtual string OnReplaceTemplate (string prefix, string template)
		{
			return template.Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (Topic)), Topic)
				           .Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (TopicReceive)), TopicReceive);
		}

		protected virtual void OnMessageReceived (string topic, byte[] message)
		{

		}

		public void WriteLog (string message) 
		{
			Console.WriteLine($"[Net][{Accessory.Name}]{message}");
		}

		public virtual void Dispose()
		{
			
		}
	}
}
