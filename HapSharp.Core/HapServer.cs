/*
 * The actual HAP server that iOS devices talk to.
 *
 * Notes
 * -----
 * It turns out that the IP-based version of HomeKit's HAP protocol operates over a sort of pseudo-HTTP.
 * Accessories are meant to host a TCP socket server that initially behaves exactly as an HTTP/1.1 server.
 * So iOS devices will open up a long-lived connection to this server and begin issuing HTTP requests.
 * So far, this conforms with HTTP/1.1 Keepalive. However, after the "pairing" process is complete, the
 * connection is expected to be "upgraded" to support full-packet encryption of both HTTP headers and data.
 * This encryption is NOT SSL. It is a customized ChaCha20+Poly1305 encryption layer.
 *
 * Additionally, this "HTTP Server" supports sending "event" responses at any time without warning. The iOS
 * device simply keeps the connection open after it's finished with HTTP request/response traffic, and while
 * the connection is open, the server can elect to issue "EVENT/1.0 200 OK" HTTP-style responses. These are
 * typically sent to inform the iOS device of a characteristic change for the accessory (like "Door was Unlocked").
 *
 * See eventedhttp.js for more detail on the implementation of this protocol.
 *
 * @event 'listening' => function() { }
 *        Emitted when the server is fully set up and ready to receive connections.
 *
 * @event 'identify' => function(callback(err)) { }
 *        Emitted when a client wishes for this server to identify itself before pairing. You must call the
 *        callback to respond to the client with success.
 *
 * @event 'pair' => function(username, publicKey, callback(err)) { }
 *        This event is emitted when a client completes the "pairing" process and exchanges encryption keys.
 *        Note that this does not mean the "Add Accessory" process in iOS has completed. You must call the
 *        callback to complete the process.
 *
 * @event 'verify' => function() { }
 *        This event is emitted after a client successfully completes the "verify" process, thereby authenticating
 *        itself to an Accessory as a known-paired client.
 *
 * @event 'unpair' => function(username, callback(err)) { }
 *        This event is emitted when a client has requested us to "remove their pairing info", or basically to unpair.
 *        You must call the callback to complete the process.
 *
 * @event 'accessories' => function(callback(err, accessories)) { }
 *        This event is emitted when a client requests the complete representation of Accessory data for
 *        this Accessory (for instance, what services, characteristics, etc. are supported) and any bridged
 *        Accessories in the case of a Bridge Accessory. The listener must call the provided callback function
 *        when the accessory data is ready. We will automatically JSON.stringify the data.
 *
 * @event 'get-characteristics' => function(data, events, callback(err, characteristics), remote, connectionID) { }
 *        This event is emitted when a client wishes to retrieve the current value of one or more characteristics.
 *        The listener must call the provided callback function when the values are ready. iOS clients can typically
 *        wait up to 10 seconds for this call to return. We will automatically JSON.stringify the data (which must
 *        be an array) and wrap it in an object with a top-level "characteristics" property.
 *
 * @event 'set-characteristics' => function(data, events, callback(err), remote, connectionID) { }
 *        This event is emitted when a client wishes to set the current value of one or more characteristics and/or
 *        subscribe to one or more events. The 'events' param is an initially-empty object, associated with the current
 *        connection, on which you may store event registration keys for later processing. The listener must call
 *        the provided callback when the request has been processed.
 */

namespace HapSharp.Core
{
	class HAP_TYPES
	{
		public const int REQUEST_TYPE = 0x00,
		USERNAME = 0x01,
		SALT = 0x02,
		PUBLIC_KEY = 0x03,
		PASSWORD_PROOF = 0x04,
		ENCRYPTED_DATA = 0x05,
		SEQUENCE_NUM = 0x06,
		ERROR_CODE = 0x07,
		PROOF = 0x0a;
	}

	class HAP_ERRORCODES
	{
		public const int INVALID_REQUEST = 0x02,
		INVALID_SIGNATURE = 0x04;
	}

	class HAP_STATUS
	{
		public const int SUCCESS = 0,
		INSUFFICIENT_PRIVILEGES = -70401,
		SERVICE_COMMUNICATION_FAILURE = -70402,
		RESOURCE_BUSY = -70403,
		READ_ONLY_CHARACTERISTIC = -70404,
		WRITE_ONLY_CHARACTERISTIC = -70405,
		NOTIFICATION_NOT_SUPPORTED = -70406,
		OUT_OF_RESOURCE = -70407,
		OPERATION_TIMED_OUT = -70408,
		RESOURCE_DOES_NOT_EXIST = -70409,
		INVALID_VALUE_IN_REQUEST = -70410;
	}

	class HapServer
	{

		System.Timers.Timer timer = new System.Timers.Timer (1000 * 60 * 10);

		public HapServer ()
		{
			// so iOS is very reluctant to actually disconnect HAP connections (as in, sending a FIN packet).
			// For instance, if you turn off wifi on your phone, it will not close the connection, instead
			// it will leave it open and hope that it's still valid when it returns to the network. And Node,
			// by itself, does not ever "discover" that the connection has been closed behind it, until a
			// potentially very long system-level socket timeout (like, days). To work around this, we have
			// invented a manual "keepalive" mechanism where we send "empty" events perodicially, such that
			// when Node attempts to write to the socket, it discovers that it's been disconnected after
			// an additional one-minute timeout (this timeout appears to be hardcoded).

			timer.Start ();
			timer.Elapsed += (s, e) => {
				//this._keepAliveTimerID = setInterval (this._onKeepAliveTimerTick.bind (this), 1000 * 60 * 10); // send keepalive every 10 minutes
			};
		}
	}
}
