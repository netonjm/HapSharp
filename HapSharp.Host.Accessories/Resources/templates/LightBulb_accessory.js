var Accessory = require('../').Accessory;
var Service = require('../').Service;
var Characteristic = require('../').Characteristic;
var uuid = require('../').uuid;

var mqtt = require('mqtt');
var client = mqtt.connect('mqtt://{{MQTT_ADDRESS}}');

var name = "{{COMPONENT_NAME}}";
var pincode = "{{COMPONENT_PINCODE}}";
var username = "{{COMPONENT_USERNAME}}";
var manufacturer = "{{COMPONENT_MANUFACTURER}}";
var model = "{{COMPONENT_MODEL}}";
var serialNumber = "{{COMPONENT_SERIALNUMBER}}";

var power = false;
var brightness= 100; // percentage
var outputLogs = false;

client.on('connect', () => {
  client.subscribe('{{COMPONENT_TOPICRECEIVE}}')
});

client.on('message', (topic, message) => {
  console.log("Received the '%s'", message);
  if(topic === '{{COMPONENT_TOPICRECEIVE}}') {
    var m = message.toString();
    if (m.startsWith ("{{COMPONENT_TOPICGET}}/")) {
        power = (m.substring("{{COMPONENT_TOPICGET}}/".length) === 'true');
        console.log("POWER IS NOW '%s'", power);
    } else if (m.startsWith ("{{COMPONENT_TOPICSETBRIGHTNESS}}/")) { 
        brightness = parseInt(m.substring("{{COMPONENT_TOPICSETBRIGHTNESS}}/".length));
        console.log("brightness IS NOW '%s'", brightness);
    }
  }
})

// Generate a consistent UUID for our light Accessory that will remain the same even when
// restarting our server. We use the `uuid.generate` helper function to create a deterministic
// UUID based on an arbitrary "namespace" and the word "light".
var lightUUID = uuid.generate('hap-nodejs:accessories:lightbulp' + name);

// This is the Accessory that we'll return to HAP-NodeJS that represents our light.
var lightAccessory = exports.accessory = new Accessory(name, lightUUID);

// Add properties for publishing (in case we're using Core.js and not BridgedCore.js)
lightAccessory.username = username;
lightAccessory.pincode = pincode;

lightAccessory
  .getService(Service.AccessoryInformation)
    .setCharacteristic(Characteristic.Manufacturer, manufacturer)
    .setCharacteristic(Characteristic.Model, model)
    .setCharacteristic(Characteristic.SerialNumber, serialNumber);

lightAccessory.on('identify', function(paired, callback) {
  if(outputLogs) console.log("Identify the '%s'", name);
  callback();
});

lightAccessory
  .addService(Service.Lightbulb, name)
  .getCharacteristic(Characteristic.On)
  .on('set', function(value, callback) {
    if(outputLogs) console.log("Turning the '%s' %s", name, value ? "on" : "off");
    this.power = value;
    client.publish('{{COMPONENT_TOPIC}}', '{{COMPONENT_TOPICSETON}}/' + value);
    callback();
  })

// We want to intercept requests for our current power state so we can query the hardware itself instead of
// allowing HAP-NodeJS to return the cached Characteristic.value.
lightAccessory
  .getService(Service.Lightbulb)
  .getCharacteristic(Characteristic.On)
  .on('get', function(callback) {
    
    // this event is emitted when you ask Siri directly whether your light is on or not. you might query
    // the light hardware itself to find this out, then call the callback. But if you take longer than a
    // few seconds to respond, Siri will give up.

     client.publish('{{COMPONENT_TOPIC}}', '{{COMPONENT_TOPICGETON}}');
    //if(this.outputLogs) console.log("'%s' is %s.", name, power ? "on" : "off");
    callback(null, power ? true : false);

  });

// also add an "optional" Characteristic for Brightness
lightAccessory
  .getService(Service.Lightbulb)
  .addCharacteristic(Characteristic.Brightness)
  .on('get', function(callback) {
    client.publish('{{COMPONENT_TOPIC}}', '{{COMPONENT_TOPICGETBRIGHTNESS}}');
    callback(null, brightness);
  })
  .on('set', function(value, callback) {
    client.publish('{{COMPONENT_TOPIC}}', '{{COMPONENT_TOPICSETBRIGHTNESS}}/' + value);
    brightness = value;
    callback();
  })
