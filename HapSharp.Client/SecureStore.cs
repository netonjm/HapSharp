using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace HapSharp.Client
{
	public class SecureStore
	{
		readonly Dictionary<string, SecureAccessoryInfo> data = new Dictionary<string, SecureAccessoryInfo> ();

		public SecureStore ()
		{
		}

		public SecureStore (string clientName)
		{
		}

		public SecureAccessoryInfo Load (string client) 
		{
			SecureAccessoryInfo result;
			data.TryGetValue (client, out result);
			return result;
		}

		public SecureAccessoryInfo Load (string clientName, string username)
		{
			Console.WriteLine ($"saving for ${clientName}/${username}");
			var item = data.FirstOrDefault (s => s.Value.ClientName == clientName && s.Value.User == username).Value;
			return item;
		}

		public void Save (SecureAccessoryInfo info)
		{
			var json = info.ToJson ();
			Console.WriteLine ($"saving for ${info._clientName}/${info.User}");
			Console.WriteLine ($"{json}");
			data.Add (info._clientName, info);
		}
	}

	class AccessoryInfo
	{
		public byte[] ltpk { get; set; }
		public string pin { get; set; }
	}

	public class SecureAccessoryInfo
	{
		internal string _username;
		internal string _clientName;
		internal byte[] _ltpk;
		internal string _pin;

		public string ClientName => _username;
		public string User => _username;
		public byte[] Ltpk => _ltpk;
		public string Pin => _pin;

		public SecureAccessoryInfo (string clientName, string username, string json)
		{
			_username = username;
			_clientName = clientName;

			if (!string.IsNullOrEmpty (json)) {
				var data = JsonConvert.DeserializeObject<AccessoryInfo> (json);
				_ltpk = data.ltpk; // Buffer.from(data.ltpk, 'base64');
				_pin = data.pin;
			} else {
				_ltpk = new byte[0];
				_pin = "";
			}
		}

		internal string ToJson ()
		{
			return JsonConvert.SerializeObject (this);
		}
	}
}
