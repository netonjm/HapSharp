using System;
using HapNet.Accessories;

namespace HapSharp
{
    public static class ExtendedMethod
    {
        public static bool ToBoolean (this string value) 
        {
            if (value == "1" || value == "0") {
                return value == "1";
            }
            return value == "true";
        }
    }

    public abstract class MessageLightDelegate : MessageDelegate
    {
        protected MessageLightDelegate (LightAccessory accessory) : base (accessory)
        {
            
        } 

        internal override void OnMessageReceived (string topic, string message)
        {
            if (message.StartsWith ("set/")) {
                OnChangePower (message.Substring ("set/".Length).ToBoolean ());
            } else{
                throw new NotImplementedException(message);
            }
        }

        protected abstract bool OnGetPower();

        protected abstract void OnChangePower(bool value);
    }

    public abstract class MessageDelegate
    {
        readonly protected Accessory accessory;

        protected MessageDelegate (Accessory accessory) 
        {
            this.accessory = accessory;
        }

        public virtual void OnIdentify () 
        {
            
        }
        
        internal virtual void OnMessageReceived (string topic, string message)
        {
           
        }

        internal virtual void OnMessageReceived (string topic, byte[] message)
        {

        }
    }
}
