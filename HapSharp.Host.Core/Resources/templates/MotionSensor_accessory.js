var Accessory = require('../').Accessory;
var Service = require('../').Service;
var Characteristic = require('../').Characteristic;
var uuid = require('../').uuid;
var mqtt = require('mqtt');
var client = mqtt.connect('mqtt://{{MQTT_ADDRESS}}');

var initialValue = false;

var detected = false;

var Sensor = {
  value: initialValue,
  getValue: function() {
    client.publish('{{COMPONENT_TOPIC}}', '{{COMPONENT_TOPICGET}}');
    return this.value;
  }
}

client.on('connect', () => {
  client.subscribe('{{COMPONENT_TOPICRECEIVE}}')
});

client.on('message', (topic, message) => {
  if(topic === '{{COMPONENT_TOPICRECEIVE}}') {
    var m = message.toString();
    if (m.startsWith ("{{COMPONENT_TOPICGET}}/")) {
        var newValue = m.substring("{{COMPONENT_TOPICGET}}/".length) == 'true';

        if (newValue != Sensor.value) {
            detected = true;
            Sensor.value = newValue;
            console.log("[{{COMPONENT_NAME}}][Get] %s", Sensor.value);
        }
    }
  }
})

// This is the Accessory that we'll return to HAP-NodeJS that represents our sensor.
var sensor = exports.accessory = new Accessory("{{COMPONENT_NAME}}", uuid.generate('hap-nodejs:accessories:motionsensor{{COMPONENT_NAME}}'));

sensor.username = "{{COMPONENT_USERNAME}}";
sensor.pincode = "{{COMPONENT_PINCODE}}";

sensor
  .getService(Service.AccessoryInformation)
    .setCharacteristic(Characteristic.Manufacturer, "{{COMPONENT_MANUFACTURER}}")
    .setCharacteristic(Characteristic.Model, "{{COMPONENT_MODEL}}")
    .setCharacteristic(Characteristic.SerialNumber, "{{COMPONENT_SERIALNUMBER}}");

sensor.on('identify', function(paired, callback) {
  console.log("[{{COMPONENT_NAME}}] Identified.");
  client.publish('{{COMPONENT_TOPIC}}', 'identify');
  callback();
});

sensor
  .addService(Service.MotionSensor, "Motion Sensor")
  .getCharacteristic(Characteristic.MotionDetected)
  .on('get', function(callback) {
    Sensor.getValue();
    callback(null, Boolean(Sensor.value));
  });

setInterval(function() {
  // updates the characteristic values so interested iOS devices can get updated sensor

    Sensor.getValue();

    if (detected) {
        sensor
        .getService(Service.MotionSensor)
        .setCharacteristic(Characteristic.MotionDetected, Sensor.value);
        detected = false;
        console.log("[{{COMPONENT_NAME}}][Detection] " + Sensor.value);
    }

}, {{COMPONENT_INTERVAL}});

console.log("[{{COMPONENT_NAME}}] Loaded.");