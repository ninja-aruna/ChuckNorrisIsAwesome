using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChuckNorrisIsAwesome
{
    /// <summary>
    ///     Using the latest version of dotnet, create a console application that consumes the following API
    ///     https://api.chucknorris.io/
    ///     While running console app should wait for user input and allow the user to retrieve a new joke by inputting j,
    ///     also retrieved jokes should be cached and the user should be able to cycle back and forward through them
    ///     by inputting p and n respectively. Format the output of the API so that it is readable.
    /// </summary>
    internal static class Program
    {
        private static async Task Main()
        {
            var host = await CreateDefaultBuilder().UseConsoleLifetime().StartAsync();

            using var serviceScope = host.Services.CreateScope();
            var provider = serviceScope.ServiceProvider;
            var workerInstance = provider.GetRequiredService<Worker>();
            await workerInstance.DoWork();

            await host.RunAsync();
        }

        private static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app => { app.AddJsonFile("appsettings.json"); })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<Worker>();
                    services.AddMemoryCache();
                    services.AddHttpClient();
                    services.AddScoped<IChuckNorrisFactRetriever, ChuckNorrisFactRetriever>();
                    services.AddScoped<ICachedJokeService, CachedJokeService>();
                    services.AddHostedService<CacheWamUpService>();
                });
        }
    }
}