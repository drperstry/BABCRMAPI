using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;

namespace BabCrm.Core.Caching
{
    public static class MemoryCacheExtensions
    {
        private static PropertyInfo EntriesInfo = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

        public static IEnumerable GetKeys(this IMemoryCache memoryCache) =>
            ((IDictionary)EntriesInfo.GetValue(memoryCache)).Keys;

        public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache) =>
            GetKeys(memoryCache).OfType<T>();
    }
}
