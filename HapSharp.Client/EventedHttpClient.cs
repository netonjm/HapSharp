using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using HapSharp.Core;
using Newtonsoft.Json;
using System.Linq;

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

		public EventedHttpClient (string host, int port = 80)
		{
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

		string Request (string method, string url, string contentType, HapBuffer buffer, (string header, string value) [] collection = null)
		{
			Console.WriteLine ($"requesting: ${method} ${url}");

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
				Stream responseStream = GetStreamForResponse (response, 200);
				string responseStr = new StreamReader (responseStream).ReadToEnd ();
				return responseStr;
			}
			return null;
		}

		public void Download (string url, string outputFilePath)
		{
			using (var client = new WebClient ()) {
				client.DownloadFile (url, outputFilePath);
			}
		}

		public string Get (string destinationUrl, (string header, string value)[] collection = null)
		{
			var client = new WebClient ();
			if (collection != null) {
				foreach (var item in collection) {
					client.Headers.Add (item.header, item.value);
				}
			}
			string response = client.DownloadString (destinationUrl);
			return response;
		}

		public string Put (string url, HapBuffer data, string contentType = "application/json", (string header, string value) [] collection = null)
		{
			Console.WriteLine ("PUTing {0}: {1}", url, data);

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

		public string Post (string url, HapBuffer buffer, string contentType = "application/json", (string header, string value)[] collection = null)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);

			request.ContentLength = buffer.Length;
			request.Method = "POST";
			//request.UserAgent = "XXX";
			request.Accept = contentType;
			request.ContentType = contentType;
			request.Headers.Add (HttpRequestHeader.ContentType, contentType);
			request.Headers.Add (HttpRequestHeader.ContentLength, buffer.Length.ToString ());
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
				Stream responseStream = GetStreamForResponse (response, 200);
				string responseStr = new StreamReader (responseStream).ReadToEnd ();
				return responseStr;
			}
			return null;
		}
	}
}
