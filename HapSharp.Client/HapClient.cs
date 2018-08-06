using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zeroconf;

namespace HapSharp.Client
{



	public class HapClientSession
	{

	}

	public class HapClient
	{
		string clientName;
		string clientId;
		string ip;
		int port;

		public HapClient (string clientName, string ip, int port) 
		{
			this.clientName = clientId = clientName;
			this.ip = ip;
			this.port = port;

		}

		object Pair (int pinProvider) 
		{
			SecureStore secureStore = new SecureStore ();
			var session = GetSession (secureStore, null);
			return null;
		}

		public HapClientSession GetSession (SecureStore secureStore, object seed) {
			//steps 
			//   1. POST the pairing request to /pair-setup
			object req = null;// Encode ();
			Console.WriteLine ("encoded request: {0}", req);

			var result = new WebClient ();
			result.Encoding = Encoding.UTF8;
			return null;
		}

		public byte[] Encode (byte[] message) {
			return null;
		}

		//public object GetAt (int index)
		//{

		//}

		//public void HandlePairStartResponse (object response, object data, object session)
		//{
		//	//2. Read the server salt and public key
		//	string salt = data [Tag.Salt], serverPublicKey = data [Tag.PublicKey];
		//	//2a validate
		//	if (salt.length != 16) {
		//		throw new Exception ("salt must be 16 bytes");
		//	}
		//	if (serverPublicKey.length != 384) {
		//		throw new Exception ("serverPublicKey must be 384 bytes (but was ${ serverPublicKey.length })`));";
		//	}

  //           Console.WriteLine (" -> s: %s", salt.ToString ('hex'));
  //           Console.WriteLine (" -> B: %s", serverPublicKey.ToString ('hex'));

		//}

		const string CLIENT_ID_NAMESPACE = "fdde9099-dae4-4a18-ad5d-a2b07b8ebb9b";

		const string PairingContentType = "application/pairing+tlv8";


		public static async Task<IReadOnlyList<IZeroconfHost>> GetServices ()
		{
			return await ZeroconfResolver.ResolveAsync ("_hap._tcp.local.");
		}
	}
}
