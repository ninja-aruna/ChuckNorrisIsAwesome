using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ChuckNorrisIsAwesome
{
    public class CachedJokeService : ICachedJokeService
    {
        private readonly List<ChuckNorrisFact> _allJokesList = new List<ChuckNorrisFact>();
        private readonly IMemoryCache _cache;
        private readonly IChuckNorrisFactRetriever _chuckNorrisFactRetriever;
        private readonly IConfiguration _configuration;


        public CachedJokeService(IMemoryCache cache, IConfiguration configuration,
            IChuckNorrisFactRetriever chuckNorrisFactRetriever)
        {
            _cache = cache;
            _configuration = configuration;
            _chuckNorrisFactRetriever = chuckNorrisFactRetriever;
        }

        public async Task<ChuckNorrisFact> GetNewJoke()
        {
            var chuckNorrisFactFromApi = await _chuckNorrisFactRetriever.GetChuckNorrisFactFromApi();
            _allJokesList.Add(chuckNorrisFactFromApi);

            return await GetOrSetValueInCache(chuckNorrisFactFromApi);
        }

        public ChuckNorrisFact GetPreviousJokeFromCache(int index)
        {
            return index < 0 ? _allJokesList.LastOrDefault() : _allJokesList.ElementAt(index);
        }

        public async Task<ChuckNorrisFact> GetNextJokeFromCache(int index)
        {
            if (index <= _allJokesList.Count - 1) return _allJokesList.ElementAt(index);

            var chuckNorrisFactFromApi = await _chuckNorrisFactRetriever.GetChuckNorrisFactFromApi();
            _allJokesList.Add(chuckNorrisFactFromApi);

            return _allJokesList.ElementAt(index);
        }

        public async Task PopulateCache(int numberOfJokes)
        {
            for (var i = 0; i < numberOfJokes; i++)
                _allJokesList.Add(await _chuckNorrisFactRetriever.GetChuckNorrisFactFromApi());
        }

        private async Task<ChuckNorrisFact> GetOrSetValueInCache(ChuckNorrisFact chuckNorrisFactFromApi)
        {
            return await _cache.GetOrSetValueAsync(chuckNorrisFactFromApi.Id,
                () => Task.FromResult(chuckNorrisFactFromApi),
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromHours(
                            Convert.ToDouble(_configuration["CacheExpirationInHours"]))
                });
        }
    }
}