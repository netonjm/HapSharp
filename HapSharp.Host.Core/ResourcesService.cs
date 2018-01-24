using System.IO;
using System.Reflection;

namespace HapSharp
{
    public static class ResourcesService
    {
        internal const int ServicePort = 51826;

        static ResourcesService()
        {

        }

        public static string GetManifestResource(Assembly assembly, string resource)
        {
            try {
                var resources = assembly.GetManifestResourceNames ();
                using (var stream = assembly.GetManifestResourceStream (resource)) {
                    using (TextReader tr = new StreamReader (stream)) {
                        return tr.ReadToEnd ();
                    };
                }
            } catch (System.Exception) {
                return null;
            }
        }
    }
}
