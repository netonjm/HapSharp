using System.Collections.Generic;
using System.Linq;

namespace HapSharp.Client
{
	public static class StringExtensions
	{
		public static List<byte> Slice (this List<byte> sender, int pos, int len)
		{
			if (pos < 0 || (pos + len) > sender.Count || len == 0) {
				return new List<byte> ();
			}
			var result = new byte[len];
			for (int i = 0; i < len; i++) {
				result[i] = sender[i + pos];
			}
			return result.ToList ();
		}

		public static List<byte> Slice (this List<byte> sender, int pos)
		{
			return Slice (sender, pos, sender.Count - pos);
		}
	}
}
