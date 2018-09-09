using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using HapSharp.Core;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

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

		public override HapBuffer OnEncoding (object test)
		{
			return HapBuffer.From (JsonConvert.SerializeObject (test));
		}
	}

	public class EventedHttpClient
	{
		static readonly List<EncoderDelegate> encoders = new List<EncoderDelegate> () {
			new EncoderHapJson ()
		};

		string host;
		int port;

		public EventedHttpClient (string host, int port = 80)
		{
			this.host = host;
			this.port = port;
		}

		public void ReadChunk ()
		{

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

		public byte[] Request (string method, string url, string contentType, HapBuffer buffer, (string header, string value) [] collection = null)
		{
			Console.WriteLine ($"requesting: ${method} ${url}");
			url = GetUrl(url, false, host, port);
			var request = (HttpWebRequest)WebRequest.Create (url);

			request.ContentLength = buffer.Length;
			request.Method = method;
			//request.UserAgent = "XXX";
			request.Accept = contentType;
			request.ContentType = contentType;
			/* Sart browser signature */
			if (collection != null) {
				foreach (var item in collection) {
					request.Headers.Add (item.header, item.value);
				}
			}

			Stream requestStream = request.GetRequestStream ();
			requestStream.Write (buffer.Data, 0, buffer.Length);
			requestStream.Close ();
			HttpWebResponse response;
			response = (HttpWebResponse)request.GetResponse ();
			if (response.StatusCode == HttpStatusCode.OK) {
				//Stream responseStream = GetStreamForResponse (response, 200);
				//string responseStr = new StreamReader (responseStream).ReadToEnd ();
				//return responseStr;
				MemoryStream memoryStream = new MemoryStream(0x10000);
				byte[] bf = new byte[0x1000];
				int bytes;
				var responseStream = response.GetResponseStream();
				while ((bytes = responseStream.Read(bf, 0, bf.Length)) > 0)
				{
					memoryStream.Write(bf, 0, bytes);
				}
				return bf;
			}
			return null;
		}

		public void Download (string url, string outputFilePath)
		{
			using (var client = new WebClient ()) {
				client.DownloadFile (url, outputFilePath);
			}
		}

		public string Get (string url, (string header, string value)[] collection = null)
		{
			url = GetUrl(url, false, host, port);
			var client = new WebClient ();
			if (collection != null) {
				foreach (var item in collection) {
					client.Headers.Add (item.header, item.value);
				}
			}
			string response = client.DownloadString (url);
			return response;
		}

		public byte[] Put (string url, HapBuffer data, string contentType = "application/json", (string header, string value) [] collection = null)
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

		string GetUrl (string url, bool isSecure, string host, int port = 80)
		{
			var builder = new StringBuilder(isSecure ? "https://" : "http://");
			builder.Append(host);
			if (port != 80) {
				builder.Append(":" + port);
			}
			builder.Append(url);
			return builder.ToString();
		}

		public byte[] Post (string url, HapBuffer buffer, string contentType = "application/json", (string header, string value)[] collection = null)
		{
			url = GetUrl(url, false, host, port);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);


			request.Method = "POST";
			//request.UserAgent = "XXX";
			request.Accept = contentType;
			request.ContentType = contentType;
			request.ContentLength = buffer.Length;
			/* Sart browser signature */
			//if (collection != null) {
			//	foreach (var item in collection) {
			//		request.Headers.Add (item.header, item.value);
			//	}
			//}

			Stream requestStream = request.GetRequestStream ();
			requestStream.Write (buffer.Data, 0, buffer.Length);
			requestStream.Close ();
			HttpWebResponse response;
			response = request.GetResponse () as HttpWebResponse;

		
			if (response.StatusCode == HttpStatusCode.OK) {
				MemoryStream memoryStream = new MemoryStream(0x10000);
				byte[] bf = new byte[0x1000];
				int bytes;
				var responseStream = response.GetResponseStream();
				while ((bytes = responseStream.Read(bf, 0, bf.Length)) > 0)
				{
					memoryStream.Write(bf, 0, bytes);
				}
				return bf;
			}
			//Stream responseStream = GetStreamForResponse (response, 200);
			//	string responseStr = new StreamReader (responseStream).ReadToEnd ();
			//	return responseStr;
			//}
			return null;
		}
	}
}
