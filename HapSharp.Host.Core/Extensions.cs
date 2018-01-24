namespace HapSharp
{
	public static class Extensions
	{
		public static bool ToBoolean (this string value)
		{
			if (value == "1" || value == "0") {
				return value == "1";
			}
			return value == "true";
		}

		public static int ToInt (this string value)
		{
			return int.Parse (value);
		}
	}
}
