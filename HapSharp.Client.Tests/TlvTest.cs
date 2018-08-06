using System;
using System.Linq;
using Greensoft.TlvLib;
using NUnit.Framework;

namespace HapSharp.Client.Tests
{
	[TestFixture ()]
	public class TlvTest
	{
		[TestCase(11, "1234")]
		[TestCase(13, "a")]
		[TestCase(15, "añada")]
		public void DecodeEncode(byte type, string data)
		{
			var dict = new System.Collections.Generic.List<(int type, string data)> {
				( type, data ),
			};

			var encoded = TLV.Encode(dict);
			var decoded = TLV.Decode(encoded);
			Assert.AreEqual(1, decoded.Count);
			Assert.IsTrue(decoded.TryGetValue(type, out Buffer buffer));
			Assert.AreEqual(data, buffer.ToString ());
		}

		[Test ()]
		public void DecodeEncodeMultiple ()
		{
			var dict = new System.Collections.Generic.List<(int type, string data)> {
				( 11, "345" ),
				( 14, "4 5 6" ),
				( 15, "33a" )
			};

			var encoded = TLV.Encode(dict);
			var decoded = TLV.Decode(encoded);
			Assert.AreEqual(3, decoded.Count);

			Buffer buffer;
			Assert.IsTrue(decoded.TryGetValue(11, out buffer));
			Assert.AreEqual("345", buffer.ToString());

			Assert.IsTrue(decoded.TryGetValue(14, out buffer));
			Assert.AreEqual("4 5 6", buffer.ToString());

			Assert.IsTrue(decoded.TryGetValue(15, out buffer));
			Assert.AreEqual("33a", buffer.ToString());
		}
	}
}
