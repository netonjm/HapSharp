using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using HapSharp.Core;
using System.Linq;
using System.Text;

namespace HapSharp.Client
{
	public class EventedHttpClient
	{
		static readonly List<EncoderDelegate> encoders = new List<EncoderDelegate> () {
			new EncoderHapJson ()
		};

		readonly string host;
		readonly int port;

		public EventedHttpClient (string host, int port)
		{
			this.host = host;
			this.port = port;
		}

		static Stream GetStreamForResponse (HttpWebResponse webResponse, int readTimeOut)
		{
			Stream stream;
			switch (webResponse.ContentEncoding.ToUpperInvariant ()) {
			case "GZIP":
				stream = new GZipStream (webResponse.GetResponseStream (), CompressionMode.Decompress);
				break;
			case "DEFLATE":
				stream = new DeflateStream (webResponse.GetResponseStream (), CompressionMode.Decompress);
				break;

			default:
				stream = webResponse.GetResponseStream ();
				stream.ReadTimeout = readTimeOut;
				break;
			}
			return stream;
		}

		public EventedHttpClientResponse Request (string method, string url, string contentType = "application/json", HapBuffer buffer = null, (string header, string value) [] collection = null)
		{
			Console.WriteLine ($"requesting: ${method} ${url}");
			url = GetUrl(url, false, host, port);

			var request = (HttpWebRequest)WebRequest.Create (url);
			request.Method = method;
			//request.UserAgent = "XXX";
			request.Accept = contentType;
			request.ContentType = contentType;
			/* Sart browser signature */

			if (buffer != null) {
				request.ContentLength = buffer.Length;

				Stream requestStream = request.GetRequestStream();
				requestStream.Write(buffer.Data, 0, buffer.Length);
				requestStream.Close();
			}

			if (collection != null)
			{
				foreach (var item in collection)
				{
					request.Headers[item.header] = item.value;
				}
			}
		
			return new EventedHttpClientResponse(request.GetResponse());
		}

		public EventedHttpClientResponse Put (string url, HapBuffer data, string contentType = "application/json", (string header, string value) [] collection = null)
		{
			Console.WriteLine ("PUTing {0}: {1}", url, data);
			url = GetUrl(url, false, host, port);
			var encoder = encoders.FirstOrDefault (s => s.ContentType == contentType);
			if (encoder != null) {
				data = encoder.OnEncoding (data.Data);
			}

			var newCol = new [] {
				("Content-Type", contentType),
					("Content-Length",data.Length.ToString ())
				};
			if (collection != null) {
				newCol = newCol.Concat (collection).ToArray ();
			}
			return Request ("PUT", url, contentType, data, newCol);
		}

		static string GetUrl (string url, bool isSecure, string host, int port = 80)
		{
			var builder = new StringBuilder(isSecure ? "https://" : "http://");
			builder.Append(host);
			if (port != 80) {
				builder.Append(":" + port);
			}
			builder.Append(url);
			return builder.ToString();
		}
	}
}
