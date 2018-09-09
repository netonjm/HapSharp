using HapSharp.Core;
using Newtonsoft.Json;

namespace HapSharp.Client
{
	public abstract class EncoderDelegate
	{
		public abstract string ContentType { get; }

		public abstract HapBuffer OnEncoding (object test);
	}

	public class EncoderHapJson : EncoderDelegate
	{
		public override string ContentType => "application/hap+json";

		public override HapBuffer OnEncoding(object test)
		{
			return HapBuffer.From(JsonConvert.SerializeObject(test));
		}
	}
}
