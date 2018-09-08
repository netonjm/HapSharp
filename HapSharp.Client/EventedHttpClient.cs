using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HapSharp.Core;

namespace HapSharp.Client
{
	public class EventedHttpClient
	{
		public EventedHttpClient (string host, int port = 80)
		{

		}

		public void ReadChunk ()
		{

		}

		void Get (string url, string [] headers)
		{
			Console.WriteLine ("GETing {0}", url);

		}

		void Request (string method, string url, string [] headers, HapBuffer data)
		{
			Console.WriteLine ($"requesting: ${method} ${url}");
		}
	}
}
