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

        public static string GetTemplate (string resourceId)
        {
            var resourceData = GetManifestResource(Assembly.GetExecutingAssembly(), resourceId);
            return resourceData;
        }

        public static string GetManifestResource(System.Reflection.Assembly assembly, string resource)
        {
            var resources = assembly.GetManifestResourceNames();
            using (var stream = assembly.GetManifestResourceStream(resource))
            {
                using (TextReader tr = new StreamReader(stream))
                {
                    return tr.ReadToEnd();
                };
            }
        }
    }
}
