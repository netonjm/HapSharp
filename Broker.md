# MQTT Mosquitto Broker Installation

## Mac

### Installing Brew (you can skip this step if you are ok)

The Mosquitto MQTT Server can be easily installed using Homebrew.  If it’s not installed on your system already, then a quick visit to the homepage will give you all you need to get going.  Homebrew is an OS X Package Manager for installing and updating non-Mac OS X utilities that are more commonly found in other variants of Linux.  To install the basic package manager run the following command.

    ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"

### Installing Mosquitto MQTT

Let’s use our new Homebrew installation to download and install the necessary Mosquitto binaries.  This will also download additional libraries required to support secure access via OpenSSL.

    brew install mosquitto

The install script finishes by providing the instructions to start the MQTT server on startup.

    ln -sfv /usr/local/opt/mosquitto/*.plist ~/Library/LaunchAgents

Finally, to save a restart, the server can be started now by running

    launchctl load ~/Library/LaunchAgents/homebrew.mxcl.mosquitto.plist

Now you can test the installation and ensure the server is running successfully.  Open a new command window and start a listener.

    mosquitto_sub -t topic/state

In another window, send a message to the listener.

    mosquitto_pub -t topic/state -m "Hello World"

## Raspberry

Install using aptitude package manager

    sudo aptitude install mosquitto

