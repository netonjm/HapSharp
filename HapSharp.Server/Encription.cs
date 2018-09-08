using System;
using System.Security.Cryptography;
using System.Text;
using HapSharp.Core;

namespace HapSharp.Server
{
	public enum EncriptionType
	{
		SHA1
	}

	public static class Curve25519
	{
		//TODO: TO IMPLEMENT
		public static void MakeSecretKey (byte[] data)
		{
			throw new NotImplementedException ();
		}
		//TODO: TO IMPLEMENT
		internal static byte [] deriveSharedSecret (byte [] secKey, byte [] pubKey)
		{
			throw new NotImplementedException ();
		}
		//TODO: TO IMPLEMENT
		internal static byte [] derivePublicKey (byte [] secKey)
		{
			throw new NotImplementedException ();
		}
	}

	public class EncriptionHelper
	{
		public static string CreateHash (EncriptionType type, string input)
		{
			if (type == EncriptionType.SHA1) {
				return CreateHashSha1 (input);
			}
			throw new NotImplementedException ();
		}

		static string CreateHashSha1 (string input)
		{
			using (SHA1Managed sha1 = new SHA1Managed ()) {
				var hash = sha1.ComputeHash (Encoding.UTF8.GetBytes (input));
				var sb = new StringBuilder (hash.Length * 2);

				foreach (byte b in hash) {
					// can be "x2" if you want lowercase
					sb.Append (b.ToString ("X2"));
				}

				return sb.ToString ();
			}
		}
	}

	public class Encription
	{
		//TODO: TO IMPLEMENT
		public static int[] FromHex (string h)
		{
			//h.replace (/ ([^ 0 - 9a - f]) / g, '');
			//var out = [], len = h.length, w = '';
			//for (var i = 0; i < len; i += 2) {
			//	w = h[i];
			//	if (((i + 1) >= len) || typeof h[i + 1] === 'undefined') {
			//		w += '0';
			//	} else {
			//		w += h[i + 1];
			//	}
			// out.push (parseInt (w, 16));
			//}
			//return out;
			throw new NotImplementedException ();
		}

		//TODO: TO IMPLEMENT
		public static byte[] generateCurve25519SecretKey ()
		{
			var secretKey = HapBuffer.Alloc (32);
			Curve25519.MakeSecretKey (secretKey.Data);
			return secretKey.Data;
		}

		//TODO: TO IMPLEMENT
		public static byte [] generateCurve25519PublicKeyFromSecretKey (byte [] secKey)
		{
			var publicKey = Curve25519.derivePublicKey (secKey);
			return publicKey;
		}

		//TODO: TO IMPLEMENT
		public static byte [] generateCurve25519SharedSecKey (byte [] secKey, byte [] pubKey)
		{
			var sharedSec = Curve25519.deriveSharedSecret (secKey, pubKey);
			return sharedSec;
		}

		//Security Layer Enc/Dec
		//TODO: TO IMPLEMENT
		public static int[] layerEncrypt (byte [] data, byte [] count, byte [] key)
		{
			//	var result = BufferShim.Alloc (0);
			//	var total = data.Length;
			//	for (var offset = 0; offset < total;) {
			//		var length = Math.Min (total - offset, 0x400);
			//		var leLength = BufferShim.Alloc (2);
			//		leLength.writeUInt16LE (length, 0);

			//		var nonce = BufferShim.Alloc (8);
			//		writeUInt64LE (count.Value++, nonce, 0);

			//		var result_Buffer = bufferShim.alloc (length);
			//		var result_mac = bufferShim.alloc (16);
			//		encryptAndSeal (key, nonce, data.slice (offset, offset + length),
			//		  leLength, result_Buffer, result_mac);

			//		offset += length;

			//		result = Buffer.Concat ([result, leLength, result_Buffer, result_mac]);
			//}
			//return result;
			throw new NotImplementedException ();
		}

		//TODO: TO IMPLEMENT
		public static int[] layerDecrypt (int[] packet,int count,int[] key,int[] extraInfo)
		{
			// Handle Extra Info
			//			if (extraInfo.leftoverData != undefined) {
			//				packet = Buffer.concat ([extraInfo.leftoverData, packet]);
			//			}

			//			var result = bufferShim.alloc (0);
			//			var total = packet.length;

			//			for (var offset = 0; offset < total;) {
			//				var realDataLength = packet.slice (offset, offset + 2).readUInt16LE (0);

			//				var availableDataLength = total - offset - 2 - 16;
			//				if (realDataLength > availableDataLength) {
			//					// Fragmented packet
			//					extraInfo.leftoverData = packet.slice (offset);
			//					break;
			//				} else {
			//					extraInfo.leftoverData = undefined;
			//				}

			//				var nonce = bufferShim.alloc (8);
			//				writeUInt64LE (count.value++, nonce, 0);

			//				var result_Buffer = bufferShim.alloc (realDataLength);

			//				if (verifyAndDecrypt (key, nonce, packet.slice (offset + 2, offset + 2 + realDataLength),
			//				  packet.slice (offset + 2 + realDataLength, offset + 2 + realDataLength + 16),
			//				  packet.slice (offset, offset + 2), result_Buffer)) {
			//					result = Buffer.concat ([result, result_Buffer]);
			//			offset += (18 + realDataLength);
			//		} else {
			//        debug ('Layer Decrypt fail!');
			//		debug ('Packet: %s', packet.toString('hex'));
			//        return 0;
			//      }
			//}

			  //return result;
			throw new NotImplementedException ();
		}

		//General Enc/Dec
		//TODO: TO IMPLEMENT
		public static int[] verifyAndDecrypt (int[] key,int[] nonce,int[] ciphertext,int[] mac,int[] addData,int[] plaintext)
		{
			//var ctx = new chacha20poly1305.Chacha20Ctx ();
			//chacha20poly1305.chacha20_keysetup (ctx, key);
			//chacha20poly1305.chacha20_ivsetup (ctx, nonce);
			//var poly1305key = bufferShim.alloc (64);
			//var zeros = bufferShim.alloc (64);
			//chacha20poly1305.chacha20_update (ctx, poly1305key, zeros, zeros.length);

			//var poly1305_contxt = new chacha20poly1305.Poly1305Ctx ();
			//chacha20poly1305.poly1305_init (poly1305_contxt, poly1305key);

			//var addDataLength = 0;
			//if (addData != undefined) {
			//	addDataLength = addData.length;
			//	chacha20poly1305.poly1305_update (poly1305_contxt, addData, addData.length);
			//	if ((addData.length % 16) != 0) {
			//		chacha20poly1305.poly1305_update (poly1305_contxt, bufferShim.alloc (16 - (addData.length % 16)), 16 - (addData.length % 16));
			//	}
			//}

			//chacha20poly1305.poly1305_update (poly1305_contxt, ciphertext, ciphertext.length);
			//if ((ciphertext.length % 16) != 0) {
			//	chacha20poly1305.poly1305_update (poly1305_contxt, bufferShim.alloc (16 - (ciphertext.length % 16)), 16 - (ciphertext.length % 16));
			//}

			//var leAddDataLen = bufferShim.alloc (8);
			//writeUInt64LE (addDataLength, leAddDataLen, 0);
			//chacha20poly1305.poly1305_update (poly1305_contxt, leAddDataLen, 8);

			//var leTextDataLen = bufferShim.alloc (8);
			//writeUInt64LE (ciphertext.length, leTextDataLen, 0);
			//chacha20poly1305.poly1305_update (poly1305_contxt, leTextDataLen, 8);

			//var poly_out = [];
			//chacha20poly1305.poly1305_finish (poly1305_contxt, poly_out);

			//if (chacha20poly1305.poly1305_verify (mac, poly_out) != 1) {
			//	debug ('Verify Fail');
			//	return false;
			//} else {
			//	var written = chacha20poly1305.chacha20_update (ctx, plaintext, ciphertext, ciphertext.length);
			//	chacha20poly1305.chacha20_final (ctx, plaintext.slice (written, ciphertext.length));
			//	return true;
			//}

			throw new NotImplementedException ();
		}

		//TODO: TO IMPLEMENT
		public static int[] encryptAndSeal (int[] key,int[] nonce, int[] plaintext,int[] addData,int[] ciphertext,int[] mac)
		{
			//var ctx = new chacha20poly1305.Chacha20Ctx ();
			//chacha20poly1305.chacha20_keysetup (ctx, key);
			//chacha20poly1305.chacha20_ivsetup (ctx, nonce);
			//var poly1305key = bufferShim.alloc (64);
			//var zeros = bufferShim.alloc (64);
			//chacha20poly1305.chacha20_update (ctx, poly1305key, zeros, zeros.length);

			//var written = chacha20poly1305.chacha20_update (ctx, ciphertext, plaintext, plaintext.length);
			//chacha20poly1305.chacha20_final (ctx, ciphertext.slice (written, plaintext.length));

			//var poly1305_contxt = new chacha20poly1305.Poly1305Ctx ();
			//chacha20poly1305.poly1305_init (poly1305_contxt, poly1305key);

			//var addDataLength = 0;
			//if (addData != undefined) {
			//	addDataLength = addData.length;
			//	chacha20poly1305.poly1305_update (poly1305_contxt, addData, addData.length);
			//	if ((addData.length % 16) != 0) {
			//		chacha20poly1305.poly1305_update (poly1305_contxt, bufferShim.alloc (16 - (addData.length % 16)), 16 - (addData.length % 16));
			//	}
			//}

			//chacha20poly1305.poly1305_update (poly1305_contxt, ciphertext, ciphertext.length);
			//if ((ciphertext.length % 16) != 0) {
			//	chacha20poly1305.poly1305_update (poly1305_contxt, bufferShim.alloc (16 - (ciphertext.length % 16)), 16 - (ciphertext.length % 16));
			//}

			//var leAddDataLen = bufferShim.alloc (8);
			//writeUInt64LE (addDataLength, leAddDataLen, 0);
			//chacha20poly1305.poly1305_update (poly1305_contxt, leAddDataLen, 8);

			//var leTextDataLen = bufferShim.alloc (8);
			//writeUInt64LE (ciphertext.length, leTextDataLen, 0);
			//chacha20poly1305.poly1305_update (poly1305_contxt, leTextDataLen, 8);

			//chacha20poly1305.poly1305_finish (poly1305_contxt, mac);
			throw new NotImplementedException ();
		}

		const ulong MAX_UINT32 = 0x00000000FFFFFFFF;
		const ulong MAX_INT53 =  0x001FFFFFFFFFFFFF;

		public static ulong onesComplement (ulong number)
		{
			number = ~number;
			  if (number < 0) {
				number = (number & 0x7FFFFFFF) + 0x80000000;
			  }
			return number;
		}

		//TODO: TO IMPLEMENT
		public static ulong[] uintHighLow (ulong number)
		{
			if (((long) number) > -1 && number <= MAX_INT53) {
				throw new IndexOutOfRangeException ();
			}

			ulong high = 0;
			ulong signbit = number & 0xFFFFFFFF;
			var low = signbit < 0 ? (number & 0x7FFFFFFF) + 0x80000000 : signbit;
			if (number > MAX_UINT32) {
				high = (number - low) / (MAX_UINT32 + 1);

			}
			return new[] { high, low };
		}

		//TODO: TO IMPLEMENT
		public static ulong[] intHighLow (int number)
		{
			if (number > -1) {
				return uintHighLow ((ulong)number);
		    }
			var hl = uintHighLow ((ulong)-number);
			var high = onesComplement (hl[0]);
			var low = onesComplement (hl[1]);
			if (low == MAX_UINT32) {
				high += 1;
				low = 0;
			} else {
				low += 1;
			}
			return new[] { high, low };
		}

		//TODO: TO IMPLEMENT
		public static ulong[] writeUInt64BE (ulong number,int buffer,ulong offset)
		{
		//	offset = offset || 0;
  //var hl = uintHighLow (number)
  //buffer.writeUInt32BE (hl[0], offset)
  //buffer.writeUInt32BE (hl[1], offset + 4)
			throw new NotImplementedException ();
		}

		//TODO: TO IMPLEMENT
		public static ulong[] writeUInt64LE (int number,int buffer,int offset)
		{
			//offset = offset || 0;
		  //var hl = uintHighLow (number)
		  //buffer.writeUInt32LE (hl[1], offset)
		  //buffer.writeUInt32LE (hl[0], offset + 4)
			throw new NotImplementedException ();
		}

	}
}
