namespace HapSharp.Client
{
	public static class PairStep
	{
		public const int StartRequest = 0x01;
		public const int StartResponse = 0x02;
		public const int VerifyRequest = 0x03; // eg client proof is wrong
		public const int VerifyResponse = 0x04;
		public const int KeyExchangeRequest = 0x05;
		public const int KeyExchangeResponse = 0x06;
	}
}
