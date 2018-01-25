﻿using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class GetMessageDelegate : MessageDelegate
	{
		const string TopicGet = "get";

		protected GetMessageDelegate (Accessory accessory) : base (accessory)
		{
		}

		public abstract int OnGetMessageReceived ();

		protected override void OnMessageReceived (string topic, string message)
		{
			if (message == TopicGet) {
				var value = OnGetMessageReceived ();
				OnSendMessage (topic, message, value);
			} else {
				throw new System.NotImplementedException (message);
			}
		}

		public override string OnReplaceTemplate (string prefix, string template)
		{
			return base.OnReplaceTemplate (prefix, template)
				       .Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (TopicGet)), TopicGet);
		}
	}

	public abstract class MessageDelegate
	{
		protected const string ReceiveTopicNode = "r";

		public event EventHandler<Tuple<string, string>> SendMessage;

		readonly protected Accessory Accessory;

		readonly public string Topic;

		internal string TopicReceive => Topic + "/" + ReceiveTopicNode;

		internal string OutputAccessoryFileName => GetNormalizedFileName (Accessory.Name) + "_accessory.js";

		protected void OnSendMessage (string topic, string message, int value)
		{
			OnSendMessage (topic, message, value.ToString ());
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
			Console.WriteLine ($"[Net][{Accessory.Name}] Identified.");
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
	}
}
