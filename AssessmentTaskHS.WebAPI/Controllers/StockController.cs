using AssessmentTaskHS.Domain.Stocks;
using AssessmentTaskHS.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentTaskHS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IRepository<StockQuote> _repository;

        public StockController(IRepository<StockQuote> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            var stocks = await _repository.GetAllAsync();
            return Ok(stocks);
        }
    }
}
