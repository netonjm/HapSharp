# HAP-Sharp



| Gitter Chat |
|---|
|  [![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/mono/HapSharp?utm_campaign=pr-badge&utm_content=badge&utm_medium=badge&utm_source=badge)



## Connected home with .NET



Using the .NET Bridge Accessory Server for HomeKit to control your home appliances and IOT devices.



### Overview



If you are an iOS user that is using HomeKit to control devices at home, you might be interested in controlling your own devices that you might have put together, purchased online or even controlling your home computers.

We have made it simple for you to build 
HapSharp is a Net implementation of the HomeKit Accessory Server.

With this host you would be able to create your own custom [HomeKit Accessory](https://www.apple.com/ios/home/accessories/) on a Raspberry Pi, Mac computer or any other platform that can run Mono.
 

![](https://d2mxuefqeaa7sj.cloudfront.net/s_FFD577E82AF51F20FD390F386BE119D266319762E84E17D789AF5DD44E377BAA_1518107528718_Accessories.png)


HapSharp also supports create your own custom accessories and share between the community installed by using NuGet.org platform.. explore now [all available nugets ready to use](https://www.nuget.org/packages?q=HapSharp)! 

Since Siri could interact with HomeKit accessories, this means we can ask Siri to control devices that don’t have support for [HomeKit](https://www.apple.com/es/ios/home/).

For instance, using just some of the available accessories, you can say:


- Siri, unlock the back door.
- Siri, open the garage door.
- Siri, turn on the coffee maker.
- Siri, turn on the living room lights.
- Siri, good morning!



### Introducing HapSharp



HapSharp project basically is a library which includes a class session to host your .net virtual accessories and expose they over HomeKit.

To use it, you will only need create a simple console, desktop, forms.. application and include the library (or nuget), and… with a few lines of code your host will be available in your network and ready to use from any application that use [HomeKit API](https://developer.apple.com/documentation/homekit) (for example the [Home App](https://www.apple.com/es/ios/home/))

To establish communication between HapSharp and HomeKit, we need a new player into the game, HAP-NodeJS. His mission is keep the communication with HomeKit and tunnel this information to our HapSharp host using MQTT.

[HAP-NodeJS](https://github.com/KhaosT/HAP-NodeJS) is a consolidated project, started at 2014, with a very active community and a solid and tested connection logic. In the future we can thing about the posibility of port to C# this layer.

HapSharp requires some previous steps before start to work with. I tried to simplify all with a [setup guide](https://github.com/netonjm/HapSharp#setup-guide).

Once your installation is done we are ready to the next step.


[![Alt text](https://img.youtube.com/vi/wNGShmgaPqI/0.jpg)](https://www.youtube.com/watch?v=wNGShmgaPqI)


**NOTES BEFORE LAUNCH**

Remember you will need run any MQTT broker before start the host server

Note: If you are experiencing issues executing, check the Troubleshooting section

Once we have all our NodeJS accessories are generated in place, we launch the NodeJS process in background.

[](https://d2mxuefqeaa7sj.cloudfront.net/s_FFD577E82AF51F20FD390F386BE119D266319762E84E17D789AF5DD44E377BAA_1517886800253_Esquema.png)



### Accessories



Right now, in this prototype there are 4 type of accessories to include in your host:

- **BridgedCore**: This an special accessory, like a hub, can host other Accessories “behind”. This way we can simply publish the Bridge (with a simple HAPServer on a single port) and all bridged Accessories will be hosted automatically.

- **Light**: This accessory exposes the behaviour of a single light, with 2 states: On/Off

- **Light with Brightness**: Same like simple light but adds a regulable brightness bar

- **Temperature sensor**: It allows handle the temperature in a specific timeout sending the calculated values to HomeKit.

- **Humidity sensor:** It allows handle the humidty in a specific timeout sending the calculated values to HomeKit.

Every new accessory inherits from Accessory class which has all the metadata required by HomeKit.



### Message Delegates



This accessories are useless without a logic, and here the Messages Delegates come into play.
They handle and configures the MQTT channel messages and transforms the MQTT calls into events.

For example:


If you want create your own managed Temperature accessory, you will need create your own CustomMessageTemperatureDelegate class:



            class CustomTemperatureMessageDelegate : GetMessageDelegate
            {
                    Random rnd = new Random (DateTime.Now.Millisecond);
    
                    public CustomTemperatureMessageDelegate (TemperatureAccessory accessory) : base (accessory)
                    {
                    }
    
                    public override int OnGetMessageReceived ()
                    {
                            var calculated = rnd.Next (20, 50);
                            Console.WriteLine ($"[Net][{Accessory.Name}][Get] {calculated}");
                            return calculated;
                    }
    
    
                    public override void OnIdentify ()
                    {
                            Console.WriteLine ($"[Net][{Accessory.Name}] Identified.");
                    }
            }


OnGetTemperature method includes the logic to calculate the temperature and returns the resulting value.

OnIdentify method is common to all delegates and is called every time HomeKit identifies the accessory.


### The Host

As we said, our most important piece, has the logic to make all this things possible! It’s the responsible of:


- Run and manage a clean environment
- Keeps alive the current .net session with all the accessories
- Generates metadata necessary and runs an internal HAP-NodeJS process
- Establish MQTT connection with a Broker and communicate with a HAP-NodeJS endpoint

But we don't have to be worry about that… The Host is available in a nuget package you’ll only need create a simple Console Application and include it.

Session instantiation.

    var session = new HapSession();

Configuring our session with Accessories and Message Delegates.


    //Bridge accessory is mandatory
    session.Add<BridgedCore, MessageBridgedCoreDelegate> ("Xamarin Net Bridge", "22:32:43:54:65:14");
    
    //Adding an example of custom temperature accessory
    session.Add<LightAccessory, CustomLightMessageDelegate> ("First Light", "AA:21:4D:87:66:78");

Starting the session


    session.Start ([Your HAP-NodeJS full path], [Optional: Broker MQTT IP/host address for communication, default value is connect to localhost]);

As we talked before, this process will kill any other running process in memory, before start the session to be sure your port is not busy.

After this, we start loading and initializing all the Accessories and Delegates, also will clean and dynamically generates HAP-NodeJS metadata into it’s accessory folder.

This accessories in node-js files are prepared to create our MQTT bridge to propagate to our .net world all events received from HAP protocol and vice versa.

The console output will inform you about what’s happening and if something goes wrong.
The host finishes session, closes communications and stops processes calling to Stop() method or Dispose. 



### Requisites


First of all you will need to configure your environment, I created a simple script to do it automagically for you

    make configure

If you want execute the current configured host, you can do it from VSforMac/VS or from a terminal window with:

    make run




### Setup Guide



First of all you will need to configure your environment, I created a simple script to do it automagically for you

    make configure

If you want execute the current configured host, you can do it from VSforMac/VS or from a terminal window with:

    make run
    


## How add the bridge accessory to your HomeKit



Open your home app in any IOS device with iOS 10+

1. Select add new accessory

[](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0016.PNG)

2. Select the use a code option

[](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0017.PNG)

3. Select your net bridge in the accessory list detected

[](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0018.PNG)

4. Type the code of your bridge (by default is: 031-45-154)

[](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0019.PNG)

5. Configure each accessory in your desired room

[](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0022.PNG)

6. Enjoy your new .Net accessories!

[](https://github.com/netonjm/HapSharp/blob/master/images/IMG_0024.PNG)


## Broker



MQTT protocol allows users publish/subscribe topics and send messages between themselves, it follows a star topology setup.

In our case, we are only to use this in a localhost scope to communicate HAP-NodeJS and HapSharp. 

Because protocol specification, they need before run and connect a Broker installed in the localhost machine (or you will get an exception), this is because we recommend install ![Mosquitto Broker](https://mosquitto.org) in your host machine, they provide a very easy installation and scripts to auto-execution on every reboot (Daemon).

[Follow this easily guide to install it.](https://github.com/netonjm/HapSharp/blob/master/Broker.md)


Another alternative is execute the Broker included in the solution (HapSharp.Host.Broker.exe). This is added to the host project and is compiled and generated every time you build the solution (the executable will be generated in HapSharp.Host.Console output directory)

You can easily execute the compiled broker executable with :


    make broker

Or 


    mono HapSharp.Host.Console/bin/Debug/HapSharp.Host.Console.exe $(PWD)/HAP-NodeJS



## Extensivity



After HAP-Sharp 0.41 version we added the feature of extensions points, and it allows read resources from external assemblies and resolve it for the file generation.

You only need create an empty library with the follow structure:

- Library
  - template (folder) foo_accessory.js -> this is your js template, build as EmbeddedResource and be sure your resource id match with it.
  - FooAccessory.cs -> based from Accessory based classes
  - FooMesssageDelegate.cs based from MessageDelegates based classes


Generate your nuget and share with the community!!!!!



## NuGeT



All host + accessories are published under NuGet.org

But maybe you want compile a not published version or a custom one… then try execute this script:


    make package


This will generate all *.nupkg files and you will only need to create a local folder and include like [a NuGet local source](https://docs.microsoft.com/es-es/visualstudio/mac/nuget-walkthrough#adding-package-sources)



## Troubleshooting



- Exception in session.Start: "Exception connecting to the broker"

This is because you don't have running any broker session in background before executing your HAP session. 

You will need execute in another terminal the Broker project included in the solution or install any other famous Broker like a service, for example in Mac or Linux Mosquitto works very well.

You have detailed in broker section how to do it

- Your port is bussy with some instance of HAP-NodeJS zombie

Show processes using the current port:

    sudo lsof -iTCP:51826 -sTCP:LISTEN


- Extra logging


    var session = new HapSession() { Debug = True };



## Advanced Topics



https://www.apple.com/ios/home/accessories/

This is a link of the protocol specification:

https://developer.apple.com/homekit/specification/

