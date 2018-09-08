namespace HapSharp.Core
{
	public static class StringExtensions
	{
		public static byte[] Slice (this byte[] sender, int pos, int len)
		{
			if (pos < 0 || (pos + len) > sender.Length || len == 0) {
				return new byte[0];
			}
			var result = new byte[len];
			for (int i = 0; i < len; i++) {
				result[i] = sender[i + pos];
			}
			return result;
		}

		public static byte[] Slice (this byte[] sender, int pos)
		{
			return Slice (sender, pos, sender.Length - pos);
		}
	}
}
