namespace HapSharp.Core
{
	public static class StringExtensions
	{
		public static byte[] Slice (this byte[] sender, int from, int to)
		{
			if (from > to  || to < from || to > sender.Length || from < 0) {
				return new byte[0];
			}
			var result = new byte[to - from];
			for (int i = from; i < to; i++) {
				result[i - from] = sender[i];
			}
			return result;
		}

		public static byte[] Slice (this byte[] sender, int pos)
		{
			return Slice (sender, pos, sender.Length);
		}
	}
}
