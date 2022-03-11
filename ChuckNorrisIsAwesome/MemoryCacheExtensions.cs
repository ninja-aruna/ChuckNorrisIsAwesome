using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ChuckNorrisIsAwesome
{
    public static class MemoryCacheExtensions
    {
        private static readonly MemoryCacheEntryOptions DefaultMemoryCacheEntryOptions
            = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
            };

        public static async Task<TObject> GetOrSetValueAsync<TObject>(
            this IMemoryCache cache,
            string key,
            Func<Task<TObject>> factory,
            MemoryCacheEntryOptions options = null)
            where TObject : class
        {
            if (cache.TryGetValue(key, out var value)) return value as TObject;

            var result = await factory();

            options ??= DefaultMemoryCacheEntryOptions;
            cache.Set(key, result, options);

            return result;
        }
    }
}