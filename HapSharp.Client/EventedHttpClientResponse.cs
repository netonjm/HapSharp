using System.IO;
using System.Net;
using HapSharp.Core;

namespace HapSharp.Client
{
	public class EventedHttpClientResponse
	{
		public byte [] Data => Buffer?.Data;
		public HapBuffer Buffer { get; private set; }
		public HttpStatusCode Code { get; private set; }

		public EventedHttpClientResponse (WebResponse rsp)
		{
			var response = (HttpWebResponse)rsp;
			if (response.StatusCode == HttpStatusCode.OK) {
				MemoryStream memoryStream = new MemoryStream (0x10000);
				byte [] bf = new byte [0x1000];
				int bytes;
				var responseStream = response.GetResponseStream ();
				while ((bytes = responseStream.Read (bf, 0, bf.Length)) > 0) {
					memoryStream.Write (bf, 0, bytes);
				}
				Buffer = new HapBuffer (bf);
				Code = response.StatusCode;
			} else {
				Buffer = HapBuffer.Alloc (0);
				Code = HttpStatusCode.BadRequest;
			}
		}
	}
}
