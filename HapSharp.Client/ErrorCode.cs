namespace HapSharp.Client
{
	public static class ErrorCode
	{
		public const int None = 0x00;
		public const int Unknown = 0x01;
		public const int AuthenticationFailed = 0x02; // eg client proof is wrong
		public const int TooManyAttempts = 0x03;
		public const int UnknownPeer = 0x04;
		public const int MaxPeer = 0x05;
		public const int MaxAuthenticationAttempts = 0x06;
	}
}
