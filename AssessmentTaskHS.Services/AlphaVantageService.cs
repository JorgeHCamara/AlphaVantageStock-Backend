using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AssessmentTaskHS.Domain.Stocks;
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

        public async Task<AlphaVantageResponse> GetStockQuotesAsync(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=5min&apikey={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API call failed with status code: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API Response: " + responseContent);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AlphaVantageResponse>(json);
        }
    }
}
