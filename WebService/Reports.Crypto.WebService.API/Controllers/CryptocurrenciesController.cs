using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reports.Crypto.WebService.DTO;
using Reports.Crypto.WebService.Services.Contracts;

namespace Reports.Crypto.WebService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptocurrenciesController : ControllerBase
    {
        private readonly ILogger<CryptocurrenciesController> _logger;
        
        private readonly ICryptocurrencyService _cryptocurrencyService;
        
        public CryptocurrenciesController(
            ILogger<CryptocurrenciesController> logger, 
            ICryptocurrencyService cryptocurrencyService)
        {
            _logger = logger;
            _cryptocurrencyService = cryptocurrencyService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(DateTime fromDate, DateTime toDate)
        {
            await _cryptocurrencyService.AddCryptocurrencyData();
            List<CryptocurrencyDisplayDataDto> result 
                = await _cryptocurrencyService.GetCryptoCurrencyData(fromDate, toDate);
            
            return Ok(result);
        }
    }
}
