using HapSharp.Accessories;
using System;

namespace HapSharp.MessageDelegates
{
	public abstract class GetMessageDelegate<T> : MessageDelegate
	{
		const string TopicGet = "get";

		protected GetMessageDelegate(Accessory accessory) : base(accessory)
		{
		}

		public abstract T OnGetMessageReceived();

		protected override void OnMessageReceived(string topic, string message)
		{
			if (message == TopicGet)
			{
				var value = OnGetMessageReceived();
				var type = value.GetType();

				if (type == typeof(bool)) {
					OnSendMessage(topic, message, (bool)(object)value);
				} else if (type == typeof(int)) {
					OnSendMessage(topic, message, (int)(object) value);
				} else {
					throw new NotImplementedException(type.ToString());
				}
			}
			else
			{
				throw new NotImplementedException(message);
			}
		}

		public override string OnReplaceTemplate(string prefix, string template)
		{
			return base.OnReplaceTemplate(prefix, template)
					   .Replace(TemplateHelper.GetTemplateTagId(prefix, nameof(TopicGet)), TopicGet);
		}
	}
}
