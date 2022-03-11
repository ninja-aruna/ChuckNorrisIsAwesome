using System;
using System.Collections;
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

            return await GetOrSetValueInCache(chuckNorrisFactFromApi);
        }

        public ChuckNorrisFact GetPreviousJokeFromCache(int index)
        {
            //TODO: check for index out of range
            return _allJokesList.ElementAt(index);
        }

        public ChuckNorrisFact GetNextJokeFromCache(int index)
        {
            //TODO: check for index out of range
            //TODO: if no item exists at next location, get new joke
            return _allJokesList.ElementAt(index);
        }

        public async Task PopulateCache(int numberOfJokes)
        {
            var chuckNorrisFactFromApi = await _chuckNorrisFactRetriever.GetChuckNorrisFactFromApi();

            for (var i = 0; i < numberOfJokes; i++)
            {
                await GetOrSetValueInCache(chuckNorrisFactFromApi);
                _allJokesList.Add(chuckNorrisFactFromApi);
            }
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