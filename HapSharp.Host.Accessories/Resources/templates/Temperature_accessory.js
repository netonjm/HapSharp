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

var outputLogs = false;

var active = true;

var TemperatureSensor = {
  currentTemperature: 0,
  getTemperature: function() {
    client.publish('{{COMPONENT_TOPIC}}', '{{COMPONENT_TOPICGET}}');
    return this.currentTemperature;
  }
}

client.on('connect', () => {
  client.subscribe('{{COMPONENT_TOPICRECEIVE}}')
});

client.on('message', (topic, message) => {
  if(topic === '{{COMPONENT_TOPICRECEIVE}}') {
    var m = message.toString();
    if (m.startsWith ("{{COMPONENT_TOPICGET}}/")) {
        var valueReceived = m.substring("{{COMPONENT_TOPICGET}}/".length);
        TemperatureSensor.currentTemperature = parseInt (valueReceived);
        console.log("Temperature: '%s'", TemperatureSensor.currentTemperature);
    }
  }
})

// Generate a consistent UUID for our light Accessory that will remain the same even when
// restarting our server. We use the `uuid.generate` helper function to create a deterministic
// UUID based on an arbitrary "namespace" and the word "light".
var lightUUID = uuid.generate('hap-nodejs:accessories:temperature' + name);

// This is the Accessory that we'll return to HAP-NodeJS that represents our light.
var sensor = exports.accessory = new Accessory(name, lightUUID);

// Add properties for publishing (in case we're using Core.js and not BridgedCore.js)
sensor.username = username;
sensor.pincode = pincode;

sensor
  .getService(Service.AccessoryInformation)
    .setCharacteristic(Characteristic.Manufacturer, manufacturer)
    .setCharacteristic(Characteristic.Model, model)
    .setCharacteristic(Characteristic.SerialNumber, serialNumber);

sensor.on('identify', function(paired, callback) {
  if(outputLogs) console.log("Identify the '%s'", name);
  client.publish('{{COMPONENT_TOPIC}}', 'identify');
  callback();
});

sensor
  .addService(Service.TemperatureSensor)
  .getCharacteristic(Characteristic.CurrentTemperature)
  .on('get', function(callback) {
    callback(null, TemperatureSensor.getTemperature());
  });

// gets our temperature reading every 6 seconds
setInterval(function() {
  // updates the characteristic values so interested iOS devices can get updated
  sensor
    .getService(Service.TemperatureSensor)
    .setCharacteristic(Characteristic.CurrentTemperature, TemperatureSensor.getTemperature());

}, {{COMPONENT_INTERVAL}});


