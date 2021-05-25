using Microsoft.AspNetCore.Mvc;
using StocksAPI.Data;
using StocksAPI.Models;
using System;
using System.Net;
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

        [HttpGet("{symbol}")]
        public async Task<IActionResult> Get(string symbol)
        {
            try
            {
                var stock = await StocksRepository.Get(symbol, HttpContext.RequestAborted);
                
                if (stock is NullStock)
                    return BadRequest($"{symbol} stock doesn't exist");

                return Ok(stock);
            }
            catch(TaskCanceledException)
            {
                return BadRequest("User cancelled");
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error when looking up {symbol} stock: {ex.Message}");
            }
        }
    }
}
