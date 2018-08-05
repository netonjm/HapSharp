using NUnit.Framework;
using System;
namespace HapSharp.Server.Tests
{
	[TestFixture ()]
	class UUID_Tests
	{
		[Test ()]
		public void Bridge_CheckInitialValues ()
		{
			var bridgeGuid = Guid.NewGuid ();
			var bridge = new Bridge ("bridge", bridgeGuid);
			Assert.IsTrue (bridge._isBridge);
		}
	}
}
