using System;
using System.Collections.Generic;

namespace HapSharp.Client
{
	public class TLV
	{
		const int MaxLen = 255;

		// Methods (see table 4-4 page 60)
		public const byte M1 = 0x01;
		public const byte M2 = 0x02;
		public const byte M3 = 0x03;
		public const byte M4 = 0x04;
		public const byte M5 = 0x05;
		public const byte M6 = 0x06;

		// Methods (see table 4-4 page 60)
		public const byte PairSetup = 0x01;
		public const byte PairVerify = 0x02;
		public const byte AddPairing = 0x03;
		public const byte RemovePairing = 0x04;
		public const byte ListPairings = 0x05;

		// TLV Values (see table 4-6 page 61)
		public const byte kTLVType_Method = 0;
		public const byte kTLVType_Identifier = 1;
		public const byte kTLVType_Salt = 2;
		public const byte kTLVType_PublicKey = 3;
		public const byte kTLVType_Proof = 4;
		public const byte kTLVType_EncryptedData = 5;
		public const byte kTLVType_State = 6;
		public const byte kTLVType_Error = 7;
		public const byte kTLVType_RetryDelay = 8;
		public const byte kTLVType_Certificate = 9;
		public const byte kTLVType_Signature = 10;
		public const byte kTLVType_Permissions = 11; // 0x00 => reg. user, 0x01 => admin
		public const byte kTLVType_FragmentData = 12;
		public const byte kTLVType_Separator = 255;

		// Errors (see table 4-5 page 60)
		public const byte kTLVError_Unknown = 0x01;
		public const byte kTLVError_Authentication = 0x02;
		public const byte kTLVError_Backoff = 0x03;
		public const byte kTLVError_MaxPeers = 0x04;
		public const byte kTLVError_MaxTries = 0x05;
		public const byte kTLVError_Unavailable = 0x06;
		public const byte kTLVError_Busy = 0x07;
	
		public static Dictionary<byte, Buffer> Decode (Buffer data)
		{
			int pos = 0;
			var ret = new Dictionary<byte, Buffer> ();

			while (data.Length - pos > 0)
			{
				var typeLen = data.Slice(pos);
				var type = typeLen.Data[0];
				var length = (int)typeLen.Data[1];

				pos += 2;

				var newData = data.Slice(pos, length);
				if (ret.TryGetValue(type, out Buffer result)) {
					ret[type] = ret[type].Append (newData);
				} else {
					ret[type] = newData;
				}
				pos += length;
			}
			return ret;
		}

		public static Buffer Encode(List<(int type, string data)> args)
		{
			Buffer encodedTLVBuffer = Buffer.Alloc(0);
			Buffer data;
			foreach (var arg in args) {

				try {
					int value = (int)new System.ComponentModel.Int32Converter ().ConvertFromString (arg.data);
					data = Buffer.From (value);
				} catch (Exception) {
					data = Buffer.From (arg.data);
				}

				// break into chunks of at most 255 bytes
				int pos = 0;
				while (data.Length - pos > 0)  
				{
					var len = Math.Min(data.Length - pos, 255);
					Console.WriteLine($"adding ${len} bytes of type ${arg.type} to the buffer starting at ${pos}");
					encodedTLVBuffer = encodedTLVBuffer.Append (Buffer.From(arg.type, len), data.Slice(pos, pos + len));
					pos += len;
				}
			}

			return encodedTLVBuffer;
		}
	}
}
