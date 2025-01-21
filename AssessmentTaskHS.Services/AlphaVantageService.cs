using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AssessmentTaskHS.Services
{
    public class AlphaVantageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AlphaVantageService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["AlphaVantage:ApiKey"]
                      ?? throw new InvalidOperationException("API key not configured.");
        }

        public async Task<string> GetStockQuotesAsync(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=5min&apikey={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
