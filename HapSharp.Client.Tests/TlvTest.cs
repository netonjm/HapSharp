using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace HapSharp.Client.Tests
{
	[TestFixture()]
	public class BufferTest
	{
		const string longText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam gravida nec ex in finibus. Cras enim magna, fringilla mattis urna et, congue porta nulla. In vitae ex interdum neque congue semper id at tellus. Nam placerat nulla sed vestibulum egestas. Etiam ac elit scelerisque augue finibus dictum. Pellentesque accumsan hendrerit orci at egestas. Vestibulum imperdiet nec magna quis aliquam. Nunc pellentesque, orci sed tristique imperdiet, tellus nunc vulputate dolor, at feugiat nibh nulla eu ex. Mauris in lobortis odio. Vivamus rutrum est velit, vitae euismod elit luctus eu. Ut maximus, ex ac fringilla porta, sapien erat lacinia enim, non hendrerit urna arcu ut neque. Vestibulum commodo sagittis mauris ultricies sagittis.";

		[Test()]
		public void CreateFromText()
		{
			var buffer = new Buffer(longText);
			Assert.AreEqual(longText, buffer.ToString());
		}

		[Test()]
		public void AppendBuffer()
		{
			string[] data = { "hola", "adios"};
			Buffer buffer = Buffer.Alloc(0);
			foreach (var item in data) {
				buffer = buffer.Append (new Buffer(item));
			}
			Assert.AreEqual(string.Join("", data), buffer.ToString());
		}


		[Test()]
		public void SliceBuffer()
		{
			string[] data = { "hola", "adios"};
			Buffer buffer = Buffer.Alloc(0);
			foreach (var item in data) {
				buffer = buffer.Append (new Buffer(item));
			}
			Assert.AreEqual(string.Join("", data), buffer.ToString());
		}
	}

	[TestFixture ()]
	public class TlvTest
	{
		[Test ()]
		public void DecodeEncodeMultiple ()
		{
			var dict = new System.Collections.Generic.List<(int type, string data)> {
				( 11, "345a" ),
				( 14, "4 5 6a" ),
				( 15, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam gravida nec ex in finibus. Cras enim magna, fringilla mattis urna et, congue porta nulla. In vitae ex interdum neque congue semper id at tellus. Nam placerat nulla sed vestibulum egestas. Etiam ac elit scelerisque augue finibus dictum. Pellentesque accumsan hendrerit orci at egestas. Vestibulum imperdiet nec magna quis aliquam. Nunc pellentesque, orci sed tristique imperdiet, tellus nunc vulputate dolor, at feugiat nibh nulla eu ex. Mauris in lobortis odio. Vivamus rutrum est velit, vitae euismod elit luctus eu. Ut maximus, ex ac fringilla porta, sapien erat lacinia enim, non hendrerit urna arcu ut neque. Vestibulum commodo sagittis mauris ultricies sagittis." ),
			};

			var encoded = TLV.Encode(dict);
			var decoded = TLV.Decode(encoded);
			Assert.AreEqual(3, decoded.Count);

			Buffer buffer;

			Assert.IsTrue(decoded.TryGetValue(11, out buffer));
			Assert.AreEqual("345a", buffer.ToString());

			Assert.IsTrue(decoded.TryGetValue(14, out buffer));
			Assert.AreEqual("4 5 6a", buffer.ToString());

			Assert.IsTrue(decoded.TryGetValue(15, out buffer));
			Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam gravida nec ex in finibus. Cras enim magna, fringilla mattis urna et, congue porta nulla. In vitae ex interdum neque congue semper id at tellus. Nam placerat nulla sed vestibulum egestas. Etiam ac elit scelerisque augue finibus dictum. Pellentesque accumsan hendrerit orci at egestas. Vestibulum imperdiet nec magna quis aliquam. Nunc pellentesque, orci sed tristique imperdiet, tellus nunc vulputate dolor, at feugiat nibh nulla eu ex. Mauris in lobortis odio. Vivamus rutrum est velit, vitae euismod elit luctus eu. Ut maximus, ex ac fringilla porta, sapien erat lacinia enim, non hendrerit urna arcu ut neque. Vestibulum commodo sagittis mauris ultricies sagittis.".Substring(0, 255), buffer.ToString());
		}

		[Test ()]
		public void EncodeHex ()
		{
			string hex = "7F";
			var data = new List<(int, string)> (){
				( 5, "0x" + hex )
			};
			var encoded = TLV.Encode (data);
			var decoded = TLV.Decode (encoded);

			string hexValue = decoded.Values.FirstOrDefault ().Data.FirstOrDefault ().ToString ("X");
			Assert.AreEqual (hex, hexValue);
		}
	}
}
