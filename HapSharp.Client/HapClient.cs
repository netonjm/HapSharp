using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HapSharp.Core;
using Zeroconf;

namespace HapSharp.Client
{
	public class HapClient
	{
		const string CLIENT_ID_NAMESPACE = "fdde9099-dae4-4a18-ad5d-a2b07b8ebb9b";
		const string PairingContentType = "application/pairing+tlv8";

		string clientName;
		string clientId;
		string ip;
		int port;

		EventedHttpClient session;

		bool _isAuthenticated = false;
		bool _authPending = false;

		public HapClient (string clientName, string ip, int port)
		{
			this.clientName = clientId = clientName;
			this.ip = ip;
			this.port = port;
			session = new EventedHttpClient (ip, port);
		}

		public void _verifyPairing ()
		{
			var clientId = this.clientId;
			var secureStore = new SecureStore (this.clientName);

			// generate new encryption keys for this session
			var privateKey = HapBuffer.Alloc (32);
			//Sodium.randombytes_buf (privateKey);
		}

		public byte [] Encode (byte [] message)
		{
			return null;
		}
	
		public static async Task<IReadOnlyList<IZeroconfHost>> GetServices ()
		{
			return await ZeroconfResolver.ResolveAsync ("_hap._tcp.local.");
		}

		public void Identify () => Get ("/identify");

		public void ListAccessories () 
		{
			var text = Get ("/accessories");
		}

		void Post (string url, HapBuffer req, string contentType, EventHandler postHandler = null) 
		{

		}

		string Get (string url)
		{
			return "";
		}

		void EnsureAuthenticated ()
		{
			if (_isAuthenticated) {

			} else if (_authPending) {

			} else {

			}
		}

		public void Pair (string pin)
		{
			var clientId = this.clientId;
			var secureStore = new SecureStore (this.clientName);

			//var session = GetSession (secureStore, "");
			// steps:
			//   1. POST the pairing request to /pair-setup
			var req = TLV.Encode (
					   (Tag.PairingMethod, "0"),
					   (Tag.Sequence, PairStep.StartRequest.ToString ())
					 );
			Console.WriteLine ("encoded request: {0}", req);

			//session.http.post 
			Post ("/pair-setup", req, PairingContentType);
		}
	}
}
