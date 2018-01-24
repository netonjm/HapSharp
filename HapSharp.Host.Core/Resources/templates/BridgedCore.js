var fs = require('fs');
var path = require('path');
var storage = require('node-persist');
var uuid = require('./').uuid;
var Bridge = require('./').Bridge;
var Accessory = require('./').Accessory;
var accessoryLoader = require('./lib/AccessoryLoader');
var mqtt = require('mqtt');

var client = mqtt.connect('mqtt://{{MQTT_ADDRESS}}')

// Initialize our storage system
storage.initSync();

// Start by creating our Bridge which will host all loaded Accessories
var bridge = new Bridge('{{COMPONENT_NAME}}', uuid.generate("{{COMPONENT_NAME}}"));

// Listen for bridge identification event
bridge.on('identify', function(paired, callback) {
  client.publish('{{COMPONENT_TOPIC}}', 'identify');
  console.log("[{{COMPONENT_NAME}}] Identified.");
  callback(); // success
});

// Load up all accessories in the /accessories folder
var dir = path.join(__dirname, "accessories");
var accessories = accessoryLoader.loadDirectory(dir);

// Add them all to the bridge
accessories.forEach(function(accessory) {
  bridge.addBridgedAccessory(accessory);
});

// Publish the Bridge on the local network.
bridge.publish({
  username: "{{COMPONENT_USERNAME}}",
  port: {{COMPONENT_PORT}},
  pincode: "{{COMPONENT_PINCODE}}",
  category: Accessory.Categories.BRIDGE
});

console.log("[{{COMPONENT_NAME}}] Loaded.");