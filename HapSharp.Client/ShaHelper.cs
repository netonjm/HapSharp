using System;
using HapSharp.Core;
using System.Security.Cryptography;
using System.Text;

namespace HapSharp.Client
{
	public static class ShaHelper
	{
		public static byte[] ToSha512Hash (this byte[] data)
		{
			using (SHA512 shaM = new SHA512Managed ()) {
				return shaM.ComputeHash (data);
			}
		}

		public static byte[] ToByteArray (this int data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByteArray (this string data)
		{
			return Encoding.UTF8.GetBytes (data);
		}

		public static int ToInt (this HapBuffer buffer)
		{
			return ToInt (buffer.Data);
		}

		public static int ToInt (this byte[] hex)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse (hex);
			return BitConverter.ToInt32 (hex, 0);
		}
	}
}
