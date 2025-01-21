using AssessmentTaskHS.Domain.Stocks;
using AssessmentTaskHS.Repository.Interfaces;
using AssessmentTaskHS.Services;
using System.Globalization;

namespace AssessmentTaskHS.WorkerService.Jobs
{
    public class StockUpsertJob
    {
        private readonly IRepository<StockQuote> _repository;
        private readonly AlphaVantageService _alphaVantageService;

        private readonly List<string> _symbols = new List<string> { "IBM", "AAPL", "MSFT", "GOOGL", "NVDA", "NFLX" };

        public StockUpsertJob(IRepository<StockQuote> repository, AlphaVantageService alphaVantageService)
        {
            _repository = repository;
            _alphaVantageService = alphaVantageService;
        }

        public async Task ExecuteAsync()
        {
            foreach (var symbol in _symbols)
            {
                try
                {
                    var stockData = await _alphaVantageService.GetStockQuotesAsync(symbol);

                    var latestEntry = stockData.TimeSeries.OrderByDescending(x => x.Key).FirstOrDefault();

                    if (latestEntry.Key != null)
                    {
                        var stockQuote = new StockQuote
                        {
                            Symbol = stockData.MetaData.Symbol,
                            TimeStamp = DateTime.Parse(latestEntry.Key, CultureInfo.InvariantCulture),
                            OpenPrice = decimal.Parse(latestEntry.Value.Open, CultureInfo.InvariantCulture),
                            HighPrice = decimal.Parse(latestEntry.Value.High, CultureInfo.InvariantCulture),
                            LowPrice = decimal.Parse(latestEntry.Value.Low, CultureInfo.InvariantCulture),
                            ClosePrice = decimal.Parse(latestEntry.Value.Close, CultureInfo.InvariantCulture),
                            Volume = int.Parse(latestEntry.Value.Volume, CultureInfo.InvariantCulture)
                        };

                        await _repository.UpsertAsync(stockQuote,
                            s => s.Symbol == stockQuote.Symbol && s.TimeStamp == stockQuote.TimeStamp);
                    }
                    else
                    {
                        throw new Exception($"No data available for {symbol}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when processing {symbol}: {ex.Message}");
                }
            }
        }

    }
}
