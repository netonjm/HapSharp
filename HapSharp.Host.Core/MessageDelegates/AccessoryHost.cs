using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public class AccessoryHost
	{
		const string Prefix = "COMPONENT";

		readonly Accessory accessory;
		readonly MessageDelegate messageDelegate;

		public Accessory Accessory => accessory;
		public MessageDelegate MessageDelegate => messageDelegate;

		public AccessoryHost (Accessory accessory, MessageDelegate messageDelegate)
		{
			this.accessory = accessory;
			this.messageDelegate = messageDelegate;
		}

		public string OnReplaceTemplate (string template) 
		{
			var result = messageDelegate.OnReplaceTemplate (Prefix, template);
			return accessory.OnReplaceTemplate (Prefix, result);
		}
	}
}
