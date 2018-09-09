using System;
using System.Text;
using System.Linq;

namespace HapSharp.Core
{
	public class HapBuffer
	{
		readonly public byte[] Data;
		public int position;

		public int Length => Data.Length;

		public HapBuffer (string text) : this (Encoding.UTF8.GetBytes(text))
		{

		}

		public HapBuffer (int size)
		{
			Data = new byte[size];
		}

		public HapBuffer RestAll ()
		{
			throw new NotImplementedException ();
		}

		public HapBuffer (byte[] slice)
		{
			Data = slice;
		}

		public override string ToString ()
		{
			return Encoding.UTF8.GetString (Data);
		}

		public static HapBuffer From (int type, int len)
		{
			return new HapBuffer (new [] { (byte)type, (byte)len });
		}

		public static HapBuffer From (string data)
		{
			var array = Encoding.UTF8.GetBytes (data);
			return ToBuffer (array);
		}

		public static HapBuffer From (int data)
		{
			var array = (byte)data;
			return ToBuffer (array);
		}

		static HapBuffer ToBuffer (byte array)
		{
			return ToBuffer (new [] { array });
		}

		static HapBuffer ToBuffer (byte[] array)
		{
			var buff = new HapBuffer (array.Length);
			for (int i = 0; i < array.Length; i++) {
				buff.Data[i] = array[i];
			}
			return buff;
		}

		public HapBuffer Append (HapBuffer newData)
		{

			var list = new byte[Data.Length + newData.Length];
			int i = 0;
			foreach (var item in Data) {
				list[i++] = item;
			}
			foreach (var item in newData.Data) {
				list[i++] = item;
			}
			return new HapBuffer (list);
		}

		public HapBuffer Slice (int pos, int len)
		{
			return new HapBuffer (Data.Slice(pos, len));
		}

		public HapBuffer Slice (int pos)
		{
			return new HapBuffer (Data.Slice(pos));
		}

		public static HapBuffer Alloc (int pos)
		{
			return new HapBuffer (pos);
		}

		public HapBuffer Append (HapBuffer buffer1, HapBuffer buffer2)
		{
			var buf = new HapBuffer (Data);
			buf = buf.Append (buffer1);
			buf = buf.Append (buffer2);
			return buf;
		}
	}
}
