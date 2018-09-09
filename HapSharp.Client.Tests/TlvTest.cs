using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HapSharp.Core;
using NUnit.Framework;

namespace HapSharp.Client.Tests
{
	[TestFixture()]
	public class BufferTest
	{
		[Test()]
		public void CreateFromText()
		{
			var text = "test magic";
			var buffer = new HapBuffer(text);
			Assert.AreEqual(text, buffer.ToString());
		}

		[Test()]
		public void AppendBuffer()
		{
			string[] data = { "hola", "adios"};
			var buffer = HapBuffer.Alloc(0);
			foreach (var item in data) {
				buffer = buffer.Append (new HapBuffer (item));
			}
			Assert.AreEqual(string.Join("", data), buffer.ToString());
		}

		[Test()]
		public void SliceBuffer()
		{
			var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
			var buffer = HapBuffer.From(text).Slice (2);
			Assert.AreEqual("rem ipsum dolor sit amet, consectetur adipiscing elit", buffer.ToString());

			buffer = HapBuffer.From(text)
				.Slice(0,3);
			Assert.AreEqual("Lor", buffer.ToString());

			buffer = HapBuffer.From(text).Slice(text.Length - 1, text.Length);
			Assert.AreEqual("t", buffer.ToString());
		}
	}

	[TestFixture ()]
	public class TlvTest
	{
		const string longText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam gravida nec ex in finibus. Cras enim magna, fringilla mattis urna et, congue porta nulla. In vitae ex interdum neque congue semper id at tellus. Nam placerat nulla sed vestibulum egestas. Etiam ac elit scelerisque augue finibus dictum. Pellentesque accumsan hendrerit orci at egestas. Vestibulum imperdiet nec magna quis aliquam. Nunc pellentesque, orci sed tristique imperdiet, tellus nunc vulputate dolor, at feugiat nibh nulla eu ex. Mauris in lobortis odio. Vivamus rutrum est velit, vitae euismod elit luctus eu. Ut maximus, ex ac fringilla porta, sapien erat lacinia enim, non hendrerit urna arcu ut neque. Vestibulum commodo sagittis mauris ultricies sagittis.";

		[Test()]
		public void DecodeEncodeLong ()
		{
			var dict = new (int type, string data)[] {
				( 11, longText ),
			};

			var encoded = TLV.Encode(dict);
			var decoded = TLV.Decode(encoded);
			Assert.AreEqual(1, decoded.Count);

			HapBuffer buffer;
			Assert.IsTrue(decoded.TryGetValue(11, out buffer));
			Assert.AreEqual(longText, buffer.ToString());
		}

		[Test ()]
		public void DecodeEncodeMultiple ()
		{
			var dict = new (int type, string data)[] {
				( 11, "Hello" ),
				( 14, "Bye" ),
				( 15, "Lorem ipsum dolor sit amet" ),
			};

			var encoded = TLV.Encode(dict);

			var textEncoded = encoded.ToString ();
			foreach (var item in dict) {
				Assert.IsTrue(textEncoded.Contains(item.data), item.data + " : " + textEncoded);
			}

			var decoded = TLV.Decode (encoded);
			Assert.AreEqual (dict.Length, decoded.Count);

			foreach (var item in dict) {
				HapBuffer buffer;
				Assert.IsTrue(decoded.TryGetValue((byte) item.type, out buffer));
				Assert.AreEqual(item.data, buffer.ToString ());
			}
		}

		[Test ()]
		public void EncodeSingleFields1Byte ()
		{
			var data = new (int, int)[]  { ( 0x01, 0x7f ) };
			var enc = TLV.Encode (data);
			Assert.AreEqual (new byte [] { 0x01, 0x01, 0x7f }, enc.Data);
		}

		[Test ()]
		public void EncodeMultipleFields1Byte ()
		{
			var data = new (int, int)[] { 
				(0x01, 0x7f),
				(0x02, 0x91)
			};
			var enc = TLV.Encode (data);
			Assert.AreEqual (new byte [] { 0x01, 0x01, 0x7f, 0x02, 0x01, 0x91 }, enc.Data);
		}

		public static string ByteArrayToString (byte [] ba)
		{
			StringBuilder hex = new StringBuilder (ba.Length * 2);
			foreach (byte b in ba)
				hex.AppendFormat ("{0:x2}", b);
			return hex.ToString ();
		}

		[Test ()]
		public void EncodeSingleFieldsSeveralBytes ()
		{
			var enc = TLV.Encode ((0x01, HapBuffer.From ("foobar")));
			Assert.AreEqual (new byte [] { 0x01, 0x06, 0x66, 0x6F, 0x6F, 0x62, 0x61, 0x72 }, enc.Data);
		}

		[Test()]
		public void EncodeMultipleFieldsSeveralBytes()
		{
			var enc = TLV.Encode((0x01, HapBuffer.From("foobar")), (0x02, HapBuffer.From("bazquux")));
			Assert.AreEqual(new byte[] {  0x01, 0x06, 0x66, 0x6F, 0x6F, 0x62, 0x61, 0x72,
						0x02, 0x07, 0x62, 0x61, 0x7A, 0x71, 0x75, 0x75, 0x78 }, enc.Data);
		}

		[Test()]
		public void EncodeSingleFieldsLoadOfBytes()
		{
			var str = new string(' ', 384);
			var enc = TLV.Encode((0x01, HapBuffer.From(str)));
			var result = HapBuffer.Alloc (0)
			.Append(
				new HapBuffer(new byte[] { 0x01, 0xFF }),
				HapBuffer.From(new string(' ', 255)),
				new HapBuffer(new byte[] { 0x01, 0x81 }),
				HapBuffer.From(new string(' ', 129))
				);
			Assert.AreEqual(enc.Data, enc.Data);
		}

		[Test()]
		public void EncodeMultipleFieldsLoadOfBytes()
		{
			var str1 = new string(' ', 384);
			var str2 = new string('x', 384);

			var enc = TLV.Encode(
			(0x01, HapBuffer.From(str1)),
			(0x02, HapBuffer.From(str2))
				);

			var result = HapBuffer.Alloc(0)
			.Append(
				new HapBuffer(new byte[] { 0x01, 0xFF }), HapBuffer.From(new string(' ', 255)),
				new HapBuffer(new byte[] { 0x01, 0x81 }), HapBuffer.From(new string(' ', 129)),
				new HapBuffer(new byte[] { 0x02, 0xFF }), HapBuffer.From(new string(' ', 255)),
				new HapBuffer(new byte[] { 0x02, 0x81 }), HapBuffer.From(new string(' ', 129))
				);
			Assert.AreEqual(enc.Data, enc.Data);
		}

		void AssertDictionary (Dictionary<byte, HapBuffer> init, Dictionary<byte, HapBuffer> second)
		{
			Assert.AreEqual(init.Count, second.Count);

			foreach (var item in init)
			{
				HapBuffer buffer;
				Assert.IsTrue(second.TryGetValue(item.Key, out buffer));
				Assert.AreEqual(item.Value.Data, buffer.Data);
			}
		}

		[Test()]
		public void DecodeSingleFieldsOfByte ()
		{
			var test = new HapBuffer(new byte[] { 0x01, 0x01, 0x7f });
			var decoded = TLV.Decode(test);

			var result = new Dictionary<byte, HapBuffer> {
				{ 0x01, new HapBuffer (new byte [] { 0x7f }) },
			};

			AssertDictionary (result, decoded);
		}

		[Test()]
		public void DecodeMultipleFieldsOfByte()
		{
			var test = new HapBuffer(new byte[] { 0x01, 0x01, 0x7f, 0x02, 0x01, 0x91 });
			var decoded = TLV.Decode(test);

			var result = new Dictionary<byte, HapBuffer> {
				{ 0x01, new HapBuffer (new byte [] { 0x7f }) },
				{ 0x02, new HapBuffer (new byte [] { 0x91 }) },
			};

			AssertDictionary(result, decoded);
		}
	}
}
