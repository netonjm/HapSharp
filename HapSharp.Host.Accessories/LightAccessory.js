var Accessory = require('../').Accessory;
var Service = require('../').Service;
var Characteristic = require('../').Characteristic;
var uuid = require('../').uuid;
var mqtt = require('mqtt');

var client = mqtt.connect('mqtt://{{MQTT_ADDRESS}}')

var name = "{{ACCESSORY_NAME}}";
var pincode = "{{ACCESSORY_PINCODE}}";
var username = "{{ACCESSORY_USERNAME}}";
var manufacturer = "{{ACCESSORY_MANUFACTURER}}";
var model = "{{ACCESSORY_MODEL}}";
var serialNumber = "{{ACCESSORY_SERIALNUMBER}}";

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
  client.publish('/home/light', 'identify')
  callback();
});

lightAccessory
  .addService(Service.Lightbulb, LightController.name)
  .getCharacteristic(Characteristic.On)
  .on('set', function(value, callback) {
    client.publish('/home/light', 'set/'+value)
    callback();
  })
  .on('get', function(callback) {
    callback(null, LightController.getPower());
  });
