using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace StocksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        public StocksController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get(string symbol)
        {
            return Ok();
        }
    }
}
