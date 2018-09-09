using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HapSharp.Core;
using Newtonsoft.Json;
using Zeroconf;
using System.Linq;

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
		readonly EventedHttpClient client;

		bool _isAuthenticated = false;
		bool _authPending = false;

		public HapClient(string clientName, string ip, int port)
		{
			this.clientName = clientId = clientName;
			this.ip = ip;
			this.port = port;
			client = new EventedHttpClient(ip, port);
		}

		public void _verifyPairing()
		{
			var clientId = this.clientId;
			var secureStore = new SecureStore(this.clientName);

			// generate new encryption keys for this session
			var privateKey = HapBuffer.Alloc(32);
			//Sodium.randombytes_buf (privateKey);
		}

		public byte[] Encode(byte[] message)
		{
			return null;
		}

		public static async Task<IReadOnlyList<IZeroconfHost>> GetServices()
		{
			return await ZeroconfResolver.ResolveAsync("_hap._tcp.local.");
		}

		public void Identify() => client.Get("/identify");

		public void ListAccessories()
		{
			var text = client.Get("/accessories");
		}

		public void Pair(string pin)
		{
			if (string.IsNullOrEmpty(pin))
			{
				throw new Exception("pin is empty");
			}

			var clientId = this.clientId;
			var secureStore = new SecureStore(this.clientName);

			//var session = GetSession (secureStore, "");
			// steps:
			//   1. POST the pairing request to /pair-setup
			var req = TLV.Encode(
					   (TLV.kTLVType_State, TLV.M1),
					   (TLV.kTLVType_Method, TLV.PairSetup)
					 );
			Console.WriteLine("encoded request: {0}", req);

			//session.http.post 
			var response = client.Post("/pair-setup", req, PairingContentType);
			var respondeBuffer = new HapBuffer(response);

			var response_tlv = TLV.Decode(respondeBuffer);

			//# Step #3 ios --> accessory (send SRP verify request) (see page 41)
			if (!response_tlv.Keys.Contains(TLV.kTLVType_State))
			{
				Console.WriteLine("response not contains kTLVType_State");
			}

			if (response_tlv[TLV.kTLVType_State].Data[0] != TLV.M2)
			{
				Console.WriteLine("response not contains kTLVType_State");
			}

			if (response_tlv.Keys.Contains(TLV.kTLVType_Error))
			{
				Console.WriteLine("response contains kTLVType_Error");
			}

			var srp_client = new SrpClient("Pair-Setup", pin);
			srp_client.SetSalt(response_tlv[TLV.kTLVType_Salt]);
			srp_client.SetServerPublicKey(response_tlv[TLV.kTLVType_PublicKey]);
			int client_pub_key = srp_client.GetPublicKey();
			int client_proof = srp_client.GetProof();

			var stateBuffer = new HapBuffer(new byte[] { TLV.M3 });
			var publicKeyBuffer = new HapBuffer(client_pub_key.ToByteArray ());
			var proofBuffer = new HapBuffer(client_proof.ToByteArray ());

			var encode_tlv = TLV.Encode(
				   (TLV.kTLVType_State, stateBuffer),
				   (TLV.kTLVType_PublicKey, publicKeyBuffer),
				   (TLV.kTLVType_Proof, proofBuffer)
					 );

			var postResponse = client.Post("/pair-setup", encode_tlv, PairingContentType);

			response_tlv = TLV.Decode(new HapBuffer(postResponse));

			// Step #5 ios --> accessory (exchange request) (see page 43)
			//# M4 Verification (page 43)

			Console.WriteLine("encoded request: ");
		}
	}
}
