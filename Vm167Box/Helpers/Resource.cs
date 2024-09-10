using System.Reflection;

namespace Vm167Box.Helpers
{
    public static class Resource
    {
        public static object? FindResource(this ResourceDictionary mainResourceDictionary, string resourceKey)
        {
            if (mainResourceDictionary == null)
            {
                throw new ArgumentNullException(nameof(mainResourceDictionary));
            }

            var mergedResources = mainResourceDictionary
                .GetType()
                .GetProperty("MergedResources", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(mainResourceDictionary);

            if (mergedResources == null)
            {
                return null;
            }

            var dict = ((IEnumerable<KeyValuePair<string, object>>)mergedResources)
                .ToDictionary(x => x.Key, y => y.Value);

            if (dict?.ContainsKey(resourceKey) == true)
            {
                return dict[resourceKey];
            }

            return null;
        }
    }
}
