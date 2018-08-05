namespace HapSharp.Client
{
	public static class VerifyStep
	{
		public const int StartRequest = 0x01;
		public const int StartResponse = 0x02;
		public const int FinishRequest = 0x03; // eg client proof is wrong
		public const int FinishResponse = 0x04;
	}
}
