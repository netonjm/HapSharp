using System;

namespace HapSharp.Server
{
	public static class OperationExpensions
	{
		public static int GreaterSignedOperator (this int sender, int value)
		{
			return sender >> value;
		}
		public static int MinorSignedOperator (this int sender, int value)
		{
			return sender << value;
		}

		public static int MinorShiftRightOperator (this int sender, int value)
		{
			return (int)(((uint)sender) << value);
		}

		public static int GreaterShiftRightOperator (this int sender, int value)
		{
			return (int)(((uint)sender) >> value);
		}

		/// <summary>
		/// Get the array slice between the two indexes.
		/// ... Inclusive for start index, exclusive for end index.
		/// </summary>
		public static T[] Slice<T> (this T[] source, int len)
		{
			// Handles negative ends.
			T[] res = new T[len];
			for (int i = 0; i < len; i++) {
				res[i] = source[i];
			}
			return res;
		}

		/// <summary>
		/// Get the array slice between the two indexes.
		/// ... Inclusive for start index, exclusive for end index.
		/// </summary>
		public static T[] Slice<T> (this T[] source, int start, int end)
		{
			// Handles negative ends.
			if (end < 0) {
				end = source.Length + end;
			}
			int len = end - start;

			// Return new array.
			T[] res = new T[len];
			for (int i = 0; i < len; i++) {
				res[i] = source[i + start];
			}
			return res;
		}


		/// <summary>
		/// Get the array slice between the two indexes.
		/// ... Inclusive for start index, exclusive for end index.
		/// </summary>
		public static void Copy (this Array source, Array target, int targetStart, int sourceStart, int sourceEnd)
		{
			Array.Copy (source, sourceStart, target, targetStart, sourceEnd - sourceStart);
		}

		/// <summary>
		/// Get the array slice between the two indexes.
		/// ... Inclusive for start index, exclusive for end index.
		/// </summary>
		public static int[] Concat (this int[] x, int[] y)
		{
			var z = new int[x.Length + y.Length];
			x.CopyTo (z, 0);
			y.CopyTo (z, x.Length);
			return z;
		}
	}
}
