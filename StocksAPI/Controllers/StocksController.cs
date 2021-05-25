using Microsoft.AspNetCore.Mvc;
using StocksAPI.Data;
using System.Threading.Tasks;

namespace StocksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStocksRepository StocksRepository;
        public StocksController(IStocksRepository stockRepository)
        {
            StocksRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string symbol)
        {
            return Ok();
        }
    }
}
