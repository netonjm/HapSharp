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
var outputLogs = false;

client.on('connect', () => {
  client.subscribe('{{COMPONENT_TOPICRECEIVE}}')
});

client.on('message', (topic, message) => {
  console.log("Received the '%s'", message);
  if(topic === '{{COMPONENT_TOPICRECEIVE}}') {
    var m = message.toString();
    if (m.startsWith ("{{COMPONENT_TOPICGETON}}/")) {
        power = (m.substring("{{COMPONENT_TOPICGETON}}/".length) === 'true');
        console.log("[{{COMPONENT_NAME}}][Get] %s", power);
    }
  }
})

// Generate a consistent UUID for our light Accessory that will remain the same even when
// restarting our server. We use the `uuid.generate` helper function to create a deterministic
// UUID based on an arbitrary "namespace" and the word "light".
var lightUUID = uuid.generate('hap-nodejs:accessories:light' + name);

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
  console.log("[{{COMPONENT_NAME}}] Identified.");
  client.publish('{{COMPONENT_TOPIC}}', 'identify');
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
  .on('get', function(callback) {
    client.publish('{{COMPONENT_TOPIC}}', '{{COMPONENT_TOPICGETON}}');
    if(this.outputLogs) console.log("'%s' is %s.", name, power ? "on" : "off");
    callback(null, power ? true : false);
  });

console.log("[{{COMPONENT_NAME}}] Loaded.");