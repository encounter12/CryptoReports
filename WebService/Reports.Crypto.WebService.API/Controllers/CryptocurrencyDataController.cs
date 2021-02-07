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
    public class CryptocurrencyDataController : ControllerBase
    {
        private readonly ILogger<CryptocurrencyDataController> _logger;
        
        private readonly ICryptocurrencyDataService _cryptocurrencyDataService;
        
        public CryptocurrencyDataController(
            ILogger<CryptocurrencyDataController> logger, 
            ICryptocurrencyDataService cryptocurrencyService)
        {
            _logger = logger;
            _cryptocurrencyDataService = cryptocurrencyService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(DateTime fromDate, DateTime toDate)
        {
            await _cryptocurrencyDataService.AddCryptocurrencyData();
            
            IEnumerable<CryptocurrencyDisplayDataDto> result 
                = await _cryptocurrencyDataService.GetCryptocurrencyData(fromDate, toDate);
            
            return Ok(result);
        }
    }
}
