using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Reports.Crypto.WebService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptocurrenciesController : ControllerBase
    {
        private readonly ILogger<CryptocurrenciesController> _logger;

        public CryptocurrenciesController(ILogger<CryptocurrenciesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(100);
            return Ok();
        }
    }
}
