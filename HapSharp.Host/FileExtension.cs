using System.IO;

namespace HapSharp
{
	public static class FileExtension
	{
		internal static void RemoveFiles (this string path)
		{
			if (File.Exists (path))
				File.Delete (path);
		}

		internal static void RemoveHapNodeJsFiles (this string path)
		{
			//Directory.GetFiles(Path.Combine(path, "persist"), "*.*").RemoveFiles();
			Directory.GetFiles (Path.Combine (path, "accessories"), "*_accessory.js").RemoveFiles ();
			Path.Combine (path, "BridgedCore.js").RemoveFiles ();
		}

		internal static void RemoveFiles (this string[] path)
		{
			foreach (var item in path) {
				item.RemoveFiles ();
			}
		}
	}
}
