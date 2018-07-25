using NUnit.Framework;

namespace HapSharp.Core.Tests
{
	class Chacha20poly1305_Tests
	{
		[Test ()]
		public void load32_test ()
		{
			var test = new int[16];
			test[1] = 1;
			test[2] = 2;
			test[3] = 2;
			var result = Chacha20poly1305.load32 (test, 0);
			Assert.AreEqual (33685760, result);
		}

		[Test ()]
		public void store32_test ()
		{
			var test = new int[16];
			test[1] = 1;
			test[2] = 2;
			test[3] = 2;

			Chacha20poly1305.store32 (test, 0, 2);
			Assert.AreEqual (2, test[0]);
			Assert.AreEqual (0, test[1]);
			Assert.AreEqual (0, test[2]);
		}

		[Test ()]
		public void plus_test ()
		{
			var result = Chacha20poly1305.plus (5, 6);
			Assert.AreEqual (11, result);
		}
	}
}
