using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ChuckNorrisIsAwesome
{
    public class CacheWamUpService : BackgroundService
    {
        private readonly ICachedJokeService _cachedJokeService;
        private readonly IConfiguration _configuration;

        public CacheWamUpService(IConfiguration configuration, ICachedJokeService cachedJokeService)
        {
            _configuration = configuration;
            _cachedJokeService = cachedJokeService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _cachedJokeService.PopulateCache(Convert.ToInt32(_configuration["InitialNumberOfJokes"]));
                await Task.Delay(
                    new TimeSpan(Convert.ToInt32(_configuration["WarmUpDelayInHours"]), 0, 0),
                    stoppingToken);
            }
        }
    }
}