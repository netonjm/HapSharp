namespace HapSharp.Client
{
	class Sha512Builder
	{
		public byte[] Hash { get; private set; } = new byte[0];

		public byte[] Origin { get; private set; } = new byte[0];

		public Sha512Builder Update (params byte[][] data)
		{
			foreach (var item in data) {
				var list = new byte[Origin.Length + item.Length];
				int i = 0;
				foreach (var itm in Origin) {
					list[i++] = itm;
				}
				foreach (var itm in item) {
					list[i++] = itm;
				}

				Origin = list;
				Hash = Origin.ToSha512Hash ();
			}
			return this;
		}
	}
}
