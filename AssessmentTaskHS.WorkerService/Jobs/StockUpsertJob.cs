using AssessmentTaskHS.Services;

namespace AssessmentTaskHS.WorkerService.Jobs
{
    public class StockUpsertJob
    {
        private readonly AlphaVantageService _alphaVantageService;

        public StockUpsertJob(AlphaVantageService alphaVantageService)
        {
            _alphaVantageService = alphaVantageService;
        }

        public async Task ExecuteAsync()
        {
            var stockData = await _alphaVantageService.GetStockQuotesAsync("IBM");
            // Parse JSON e insira/atualize os dados no banco.
            Console.WriteLine(stockData);
        }
    }
}
