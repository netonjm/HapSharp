using System;
using HapSharp.Core.Accessories;

namespace HapSharp.Core.MessageDelegates
{
    public abstract class MessageDelegate
    {
        protected const string ReceiveTopicNode = "r";

        public event EventHandler<Tuple<string, string>> SendMessage;

        readonly internal Accessory accessory;

        readonly internal string Topic;

        internal string TopicReceive => Topic + "/" + ReceiveTopicNode;

        internal string OutputAccessoryFileName => GetNormalizedFileName(accessory.Name) + "_accessory.js";

        protected void OnSendMessage (string topic, string message)
        {
            SendMessage?.Invoke (this, new Tuple<string, string>(topic, message));
        }

        protected MessageDelegate (Accessory accessory) 
        {
            this.accessory = accessory;
            Topic = "home/" + accessory.Id;
        }

        internal string GetTemplateTagId (string prefix, string name) 
        {
            return $"{{{{{prefix}_{name.ToUpper ()}}}}}";
        }

        public virtual string GetTemplate ()
        {
            //TODO: we need a strong replace way
            return ResourcesService.GetTemplate(accessory.Template)
                                   .Replace(GetTemplateTagId (accessory.Prefix, nameof(accessory.Name)), accessory.Name)
                                   .Replace(GetTemplateTagId (accessory.Prefix, nameof(accessory.PinCode)), accessory.PinCode)
                                   .Replace(GetTemplateTagId (accessory.Prefix, nameof(accessory.UserName)), accessory.UserName)
                                   .Replace(GetTemplateTagId (accessory.Prefix, nameof(accessory.Manufacturer)), accessory.Manufacturer)
                                   .Replace(GetTemplateTagId (accessory.Prefix, nameof(accessory.SerialNumber)), accessory.SerialNumber)
                                   .Replace(GetTemplateTagId (accessory.Prefix, nameof(Topic)), Topic)
                                   .Replace(GetTemplateTagId (accessory.Prefix, nameof(TopicReceive)), TopicReceive)
                                   .Replace(GetTemplateTagId (accessory.Prefix, nameof(accessory.Model)), accessory.Model);
            
        }

        public virtual void OnIdentify () 
        {
            
        }

        string GetNormalizedFileName (string name)
        {
            return name.Replace(" ", "");
        }

        internal virtual void OnMessageReceived (string topic, string message)
        {
           
        }

        internal virtual void OnMessageReceived (string topic, byte[] message)
        {

        }
    }
}
