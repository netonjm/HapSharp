using System;
using HapSharp.Core;

namespace HapSharp.Client
{
	public class SrpClient
	{
		//# generator as defined by 3072bit group of RFC 5054
		int g = 5;
		//# modulus as defined by 3072bit group of RFC 5054
		int n = "FFFFFFFFFFFFFFFFC90FDAA22168C234C4C6628B80DC1CD129024E08n8A67CC74020BBEA63B139B22514A08798E3404DDEF9519B3CD3A431Bn302B0A6DF25F14374FE1356D6D51C245E485B576625E7EC6F44C42E9nA637ED6B0BFF5CB6F406B7EDEE386BFB5A899FA5AE9F24117C4B1FE6n49286651ECE45B3DC2007CB8A163BF0598DA48361C55D39A69163FA8nFD24CF5F83655D23DCA3AD961C62F356208552BB9ED529077096966Dn670C354E4ABC9804F1746C08CA18217C32905E462E36CE3BE39E772Cn180E86039B2783A2EC07A28FB5C55DF06F4C52C9DE2BCBF695581718n3995497CEA956AE515D2261898FA051015728E5A8AAAC42DAD33170Dn04507A33A85521ABDF1CBA64ECFB850458DBEF0A8AEA71575D060C7DnB3970F85A6E1E4C7ABF5AE8CDB0933D71E8C94E04A25619DCEE3D226n1AD2EE6BF12FFA06D98A0864D87602733EC86A64521F2B18177B200CnBBE117577A615D6C770988C0BAD946E208E24FA074E5AB3143DB5BFCnE0FD108E4B82D120A93AD2CAFFFFFFFFFFFFFFFF"
			.ToByteArray ()
			.ToInt ();

		//"""Required SRP functions to simulate an iOS HomeKit controller."""
		public SrpClient (string usermame, string password)
		{
			Username = usermame;
			Password = password;
			salt = 0;

			//self.a = int(binascii.hexlify(
			//crypt.mksalt(crypt.METHOD_SHA512)[3:].encode()), 16)
			A = Pow (g, a, n);
		}

		public string Username { get; }
		public string Password { get; }
		public int A { get; private set; }
		public int B { get; private set; }
		public int C { get; private set; }

		int salt;

		int k, u, a, x;

		int CalculateU ()
		{
			if (A == 0) {
				throw new Exception ("Client\\'s public key is missing");
			}
			if (B == 0) {
				throw new Exception ("Server\\'s public key is missing");
			}

			//TODO: WRONG # 383 * b'0' + '5'.encode()
			var aa = A.ToByteArray ();
			var bb = B.ToByteArray ();

			var uu = new Sha512Builder ()
			.Update (aa, bb)
			.Hash.ToInt ();
			return uu;
		}

		int CalculateK ()
		{
			//TODO: WRONG # 383 * b'0' + '5'.encode()
			var nn = n.ToByteArray ();
			var gg = (383 + "00" + "05").ToByteArray ();

			var kk = new Sha512Builder ()
			.Update (nn, gg)
			.Hash.ToInt ();
			return kk;
		}

		int CalculateX ()
		{
			var i = (Username + ":" + Password)
			.ToByteArray ();

			var hashValue = new Sha512Builder ()
			.Update (i)
			.Hash;

			var intHash = new Sha512Builder ()
			.Update (salt.ToByteArray (), hashValue)
			.Hash.ToInt ();
			return intHash;
		}

		public int GetSessionKey ()
		{
			var shared = GetSharedSecret ().ToByteArray ();
			var hash_value = new Sha512Builder ()
			.Update (shared).Hash.ToInt ();
			return hash_value;
		}

		internal bool VerifyServersProof (HapBuffer M)
		{
			var tmp = M.Data.ToInt ();
			var hash_value = new Sha512Builder ()
				.Update (
				A.ToByteArray (),
				GetProof ().ToByteArray (),
				GetSessionKey ().ToByteArray ()).Hash.ToInt ();

			return hash_value == tmp;
		}

		internal int GetProof ()
		{
			if (B == 0) {
				throw new Exception ("Server\\'s public key is missing");
			}
			var hn = new Sha512Builder ()
			.Update (n.ToByteArray ())
				.Hash;

			var hg = new Sha512Builder ()
		.Update (g.ToByteArray ())
			.Hash;

			for (int i = 0; i < hn.Length; i++) {
				hn[i] ^= hg[i];
			}

			var hu = new Sha512Builder ()
	.Update (Username.ToByteArray ())
		.Hash;

			var K = GetSessionKey ().ToByteArray ();


			var r = new Sha512Builder ()
				.Update (
				hn,
				hu,
				salt.ToByteArray (),
				A.ToByteArray (),
				B.ToByteArray (),
				K
				).Hash.ToInt ();
			return r;
		}

		internal int GetPublicKey ()
		{
			return Pow (g, a, n);
		}

		internal int GetSharedSecret ()
		{
			if (B == 0) {
				throw new Exception ("Server\\'s public key is missing");
			}

			var u = CalculateU ();
			var x = CalculateX ();
			var k = CalculateK ();

			var tmp1 = B - (k * Pow (g, x, n));
			var tmp2 = (a + (u * x)); // # % self.n
			return Pow (tmp1, tmp2, n);
		}

		static int Pow (int x, int y, int z)
		{
			return (int)Math.Pow (Math.Pow (x, y), z);
		}

		internal void SetSalt (HapBuffer hapBuffer)
		{
			salt = hapBuffer.ToInt ();
		}

		internal void SetServerPublicKey (HapBuffer hapBuffer)
		{
			B = hapBuffer.ToInt ();
		}
	}
}
