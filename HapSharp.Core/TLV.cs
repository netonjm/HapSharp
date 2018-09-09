using System;
using System.Collections.Generic;
using System.Linq;

namespace HapSharp.Core
{
	public class Tag
	{
		public const byte PairingMethod = 0x00;
		public const byte Username = 0x01;
		public const byte Salt = 0x02; // salt is 16 bytes long

		// could be either the SRP client public key (384 bytes) or the ED25519 public key (32 bytes), depending on context
		public const byte PublicKey = 0x03;
		public const byte Proof = 0x04; // 64 bytes
		public const byte EncryptedData = 0x05;
		public const byte Sequence = 0x06;
		public const byte ErrorCode = 0x07;
		public const byte Signature = 0x0A; // 64 bytes

		public const byte MFiCertificate = 0x09;
		public const byte MFiSignature = 0x0A;
	}

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

		public static Dictionary<byte, HapBuffer> Decode(HapBuffer data)
		{
			var ret = new Dictionary<byte, HapBuffer>();
			var leftLength = data.Length;
			var currentIndex = 0;

			for (; leftLength > 0;)
			{
				var type = data.Data[currentIndex];
				var length = data.Data[currentIndex + 1];
				currentIndex += 2;
				leftLength -= 2;

				var newData = data.Slice(currentIndex, currentIndex + length);

				if (ret.TryGetValue(type, out HapBuffer result))
				{
					ret[type] = ret[type].Append(newData);
				}
				else
				{
					ret[type] = newData;
				}

				currentIndex += length;
				leftLength -= length;
			}

			return ret;
		}

		public static HapBuffer Encode(params (int type, int data)[] args)
		{
			var ele = args.Select(s => (s.type, HapBuffer.From (s.data))).ToArray();
			return Encode(ele);
		}

		public static HapBuffer Encode(params (int type, HapBuffer data)[] args)
		{
			return Encode(args.Select(s => (s.type, s.data.ToString ())).ToArray());
		}

		public static HapBuffer Encode(params (int type, string data)[] args)
		{
			HapBuffer encodedTLVBuffer = HapBuffer.Alloc(0);
			HapBuffer data;
			foreach (var arg in args)
			{
				data = HapBuffer.From(arg.data);

				if (data.Length <= 255)
				{
					encodedTLVBuffer = encodedTLVBuffer.Append (HapBuffer.From(arg.type, arg.data.Length), data);
				}
				else
				{
					var leftLength = arg.data.Length;
					var tempBuffer = HapBuffer.Alloc(0);
					var currentStart = 0;

					for (; leftLength > 0;)
					{
						if (leftLength >= 255)
						{
							tempBuffer = tempBuffer.Append(HapBuffer.From(arg.type, 255), data.Slice(currentStart, currentStart + 255));//  Buffer.concat([tempBuffer,bufferShim.from([type,0xFF]),data.slice(currentStart, currentStart + 255)]);
							leftLength -= 255;
							currentStart = currentStart + 255;
						}
						else
						{
							tempBuffer = tempBuffer.Append(HapBuffer.From(arg.type, leftLength), data.Slice(currentStart, currentStart + leftLength)); // Buffer.concat([tempBuffer,bufferShim.from([type,leftLength]),data.slice(currentStart, currentStart + leftLength)]);
							leftLength -= leftLength;
						}
					}
					encodedTLVBuffer = encodedTLVBuffer.Append (tempBuffer);

				}
			}

			return encodedTLVBuffer;
		}
	}
}
