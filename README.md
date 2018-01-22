# HAP-Sharp
## Net bridge Accessory Server for HomeKit 

### Overview

 HapSharp it’s a .net bridge to allow you expose our awesome .net world into HomeKit protocol.

### Requisites

First of all you will need to configure your environment, I created a simple script to do it automagically for you

    make configure

If you want execute the current configured host, you can do it from VSforMac/VS or from a terminal window with:

    make run

### How it works

Because we want re-use years of work and investigation from the community, my work it’s based in HAP-NodeJS project https://github.com/KhaosT/HAP-NodeJS .

Basically, our .net data structure generates the node-js “accessory” code required by HAP-NodeJS which is the protagonist of all communication with HomeKit protocol.

The generated NodeJS accessory files are prepared to establish MQTT communication with our .Net Bridge converting this MQTT calls into .net calls event based.

### Accessories

Right now, in this prototype there are 4 type of accessories to include in your host:

* BridgedCore: This an special accessory can host other Accessories “behind”. This way we can simply publish the Bridge (with a simple HAPServer on a single port) and all bridged Accessories will be hosted automatically, instead of needed to publish every single Accessory as a separte server (this is not implemented yet).

Right now, we use this way to load all accessories created because it’s the easiest way. In the future this will be more customisable.



* **Light:** This accessory exposes the behaviour of a single light, with 2 states: On/Off

* :**Light with Brightness::** Same like simple light but adds a regulable brightness bar

* :**Temperature sensor::** It allows handle the temperature in a specific timeout sending the calculated values to HomeKit.

Every new accessory inherits from **Accessory class** which has all the metadata required by HomeKit.


### Message Delegates

This accessories are useless without a logic, and here the Messages Delegates come into play.

The Message Delegate handles and configures the MQTT channel messages and transforms the MQTT calls into events.

For example:

If you want create your own managed Temperature accessory, you will need create your own CustomMessageTemperatureDelegate class:

```
  class CustomTemperatureMessageDelegate : MessageTemperatureDelegate
    {
        public CustomTemperatureMessageDelegate (TemperatureAccessory accessory) : base (accessory)
        {
        }

        Random rnd = new Random (DateTime.Now.Millisecond);
        protected override int OnGetTemperature ()
        {
            var calculated = rnd.Next(20, 50);
            Console.WriteLine($"[Net] Temperature: {calculated}");
            return calculated;
        }

        public override void OnIdentify()
        {
            Console.WriteLine("[Net]" + accessory.Name + " identified!!");
        }
    }
```

OnGetTemperature handles the logic to calculate (or get) the temperature returns the desired temperature value.

OnIdentify method is common to all delegates and is called every time HomeKit identifies the accessory.


### The Host

The little boy who has the logic to make all this things possible is the Host. 

In order of execution it’s the responsible of:

1. Run and manage a clean environment
2. Generates all native accessory code
3. Keeps alive the current .net session
4. Management HAP-NodeJS sessions …  

### In Code

The host is represented by HapSession class which has 3 steps

Instanciate

```
var session = new HapSession();
```

Add all message delegates and accessories you want

```
//Bridge accessory is mandatory
 var bridge = new BridgedCore ("NetAwesomeBridge", "22:32:43:54:65:14");
 var bridgeDelegate = new CustomBridgedCoreMessageDelegate(bridge);
 session.Add(bridgeDelegate);

//Adding an example of custom temperature accessory
 var temperature = new TemperatureAccessory ("MyTemperature", "22:32:43:54:65:14");
 var temperatureDelegate = new CustomTemperatureMessageDelegate (temperature);
 session.Add(temperatureDelegate);
```

Start the session

```
session.Start ([Your HAP-NodeJS path], [your Broker MQTT address for communication]);
```

At this point your defined message handlers and accessories are loaded and your host knows all the necessary things to execute your HomeKit accessory hosting. 

As we talked before, this process will kill any other running process in memory, before start the session to be sure your port is not busy. 

Note: If you are experiencing issues executing, check the Troubleshooting section

After this, all native accessories will be cleaned in the HAP-NodeJS embedded project (remember you specified the path) and the host will generate the necessary native files in the corresponding folder.

The console output will inform you about what’s happening and if something goes wrong.

The host finishes session, closes communications and stops processes calling to Stop() method or Dispose.


## How add the bridge accessory to your HomeKit

Open your home app in any IOS device with iOS 10+

1. Select add new accessory

![](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0016.PNG)

2. Select the use a code option

![](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0017.PNG)

3. Select your net bridge in the accessory list detected

![](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0018.PNG)

4. Type the code of your bridge (by default is: 031-45-154)

![](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0019.PNG)

5. Configure each accessory in your desired room

![](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0022.PNG)

6. Enjoy your new .Net accessories!

![](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0024.PNG)

## Troubleshooting

* Your port is bussy with some instance of HAP-NodeJS zombie

Open Mac Terminal:

```
sudo lsof -iTCP:51826 -sTCP:LISTEN
```

* Extra logging

```
    var session = new HapSession() { Debug = True };
```
