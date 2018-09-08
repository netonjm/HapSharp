namespace HapSharp.Client
{
	public static class ErrorCode
	{
		public const uint None = 0x00;
		public const uint Unknown = 0x01;
		public const uint AuthenticationFailed = 0x02; // eg client proof is wrong
		public const uint TooManyAttempts = 0x03;
		public const uint UnknownPeer = 0x04;
		public const uint MaxPeer = 0x05;
		public const uint MaxAuthenticationAttempts = 0x06;
	}

	public static class PairStep
	{
		public const uint StartRequest = 0x01;
		public const uint StartResponse = 0x02; // eg client proof is wrong
		public const uint VerifyRequest = 0x03;
		public const uint VerifyResponse = 0x04;
		public const uint KeyExchangeRequest = 0x05;
		public const uint KeyExchangeResponse = 0x06;
	}

	public static class VerifyStep
	{
		public const uint StartRequest = 0x01;
		public const uint StartResponse = 0x02; // eg client proof is wrong
		public const uint FinishRequest = 0x03;
		public const uint FinishResponse = 0x04;
	}
}
