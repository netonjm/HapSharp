var Accessory = require('../').Accessory;
var Service = require('../').Service;
var Characteristic = require('../').Characteristic;
var uuid = require('../').uuid;
var mqtt = require('mqtt');
var client = mqtt.connect('mqtt://{{MQTT_ADDRESS}}');

var Sensor = {
  value: 0,
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
        var valueReceived = m.substring("{{COMPONENT_TOPICGET}}/".length);
        Sensor.value = parseInt (valueReceived);
        console.log("[{{COMPONENT_NAME}}][GET]: '%s'", Sensor.value);
    }
  }
})

// This is the Accessory that we'll return to HAP-NodeJS that represents our sensor.
var sensor = exports.accessory = new Accessory("{{COMPONENT_NAME}}", uuid.generate('hap-nodejs:accessories:temperature{{COMPONENT_NAME}}'));

sensor.username = "{{COMPONENT_USERNAME}}";
sensor.pincode = "{{COMPONENT_PINCODE}}";

sensor
  .getService(Service.AccessoryInformation)
    .setCharacteristic(Characteristic.Manufacturer, "{{COMPONENT_MANUFACTURER}}")
    .setCharacteristic(Characteristic.Model, "{{COMPONENT_MODEL}}")
    .setCharacteristic(Characteristic.SerialNumber, "{{COMPONENT_SERIALNUMBER}}");

sensor.on('identify', function(paired, callback) {
  console.log("[{{COMPONENT_NAME}}] identified.");
  client.publish('{{COMPONENT_TOPIC}}', 'identify');
  callback();
});

sensor
  .addService(Service.HumiditySensor)
  .getCharacteristic(Characteristic.CurrentRelativeHumidity)
  .on('get', function(callback) {
    callback(null, Sensor.getValue());
  });

setInterval(function() {
  // updates the characteristic values so interested iOS devices can get updated
  sensor
    .getService(Service.HumiditySensor)
    .setCharacteristic(Characteristic.CurrentRelativeHumidity, Sensor.getValue());
}, {{COMPONENT_INTERVAL}});

console.log("[{{COMPONENT_NAME}}] loaded.");