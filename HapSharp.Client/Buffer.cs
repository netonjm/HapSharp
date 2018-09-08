using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HapSharp.Client
{
	public class Buffer
	{
		readonly public byte[] Data;
		public int position;

		public int Length => Data.Length;

		public Buffer (string text) : this (Encoding.UTF8.GetBytes(text))
		{

		}

		public Buffer (int size)
		{
			Data = new byte[size];
		}

		public Buffer (byte[] slice)
		{
			Data = slice;
		}

		public override string ToString ()
		{
			return Encoding.UTF8.GetString (Data);
		}

		public static Buffer From (int type, int len)
		{
			return new Buffer (new [] { (byte)type, (byte)len });
		}

		public static Buffer From (string data)
		{
			var array = Encoding.UTF8.GetBytes (data);
			return ToBuffer (array);
		}

		public static Buffer From (int data)
		{
			var array = (byte)data;
			return ToBuffer (array);
		}

		static Buffer ToBuffer (byte array)
		{
			return ToBuffer (new [] { array });
		}

		static Buffer ToBuffer (byte[] array)
		{
			var buff = new Buffer (array.Length);
			for (int i = 0; i < array.Length; i++) {
				buff.Data[i] = array[i];
			}
			return buff;
		}

		public Buffer Append (Buffer newData)
		{

			var list = new byte[Data.Length + newData.Length];
			int i = 0;
			foreach (var item in Data) {
				list[i++] = item;
			}
			foreach (var item in newData.Data) {
				list[i++] = item;
			}
			return new Buffer (list);
		}

		public Buffer Slice (int pos, int len)
		{
			var slice = Data.Slice (pos, len);
			return new Buffer (slice.ToArray ());
		}

		public Buffer Slice (int pos)
		{
			var slice = Data.Slice (pos);
			return new Buffer (slice.ToArray ());
		}

		public static Buffer Alloc (int pos)
		{
			return new Buffer (pos);
		}

		public Buffer Append (Buffer buffer1, Buffer buffer2)
		{
			var buf = new Buffer (Data);
			buf = buf.Append (buffer1);
			buf = buf.Append (buffer2);
			return buf;
		}
	}
}
