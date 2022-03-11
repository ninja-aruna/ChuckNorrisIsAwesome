using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ChuckNorrisIsAwesome
{
    internal class ChuckNorrisFactRetriever : IChuckNorrisFactRetriever
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ChuckNorrisFactRetriever(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_configuration["ApiBaseUrl"]);
        }

        public async Task<ChuckNorrisFact> GetChuckNorrisFactFromApi()
        {
            var response = await _httpClient.GetAsync(_configuration["ApiEndpoint"]);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return JsonSerializer.Deserialize<ChuckNorrisFact>(responseBody, options);
        }
    }
}