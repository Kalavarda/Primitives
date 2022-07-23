using System;
using System.Reflection;

namespace Kalavarda.Primitives.WPF
{
    public static class ResourcesExtensions
    {
        public static Uri GetResourceUri(this Assembly assembly, string path)
        {
            if (path.StartsWith('/'))
                path = path[1..];
            return new Uri("pack://application:,,,/" + assembly.GetName().Name + ";component/" + path);
        }
    }
}
