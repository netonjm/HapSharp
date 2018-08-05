using System;
namespace HapSharp.Server
{
	class UUID
	{
		public Guid guid;
		readonly public string Name;
		public UUID (string name)
		{
			Name = name;
			guid = Guid.NewGuid ();
		}
	}
}
