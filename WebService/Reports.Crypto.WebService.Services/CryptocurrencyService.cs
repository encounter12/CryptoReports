using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Reports.Crypto.WebService.DAL.Repositories.Contracts;
using Reports.Crypto.WebService.DTO;
using Reports.Crypto.WebService.Services.Contracts;

namespace Reports.Crypto.WebService.Services
{
    public class CryptocurrencyService: ICryptocurrencyService
    {
        private readonly ICryptocurrencyRepository _cryptocurrencyRepository;
        
        public CryptocurrencyService(ICryptocurrencyRepository cryptocurrencyRepository)
        {
            _cryptocurrencyRepository = cryptocurrencyRepository;
        }
        
        public async Task AddCryptoCurrencyData()
        {
            var currencyCode = await _cryptocurrencyRepository.All()
                .OrderBy(c => c.Code)
                .Select(c => c.Code)
                .FirstOrDefaultAsync();
            
            var url = $"https://coinmetrics.io/newdata/{currencyCode}.csv";
            
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Stream cryptocurrencyStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(cryptocurrencyStream);
                
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = (header, index) => header.ToLower()
                };
                
                var cryptocurrencyRecords = new List<CryptocurrencyDataDto>();
                
                using (var csv = new CsvReader(reader, config))
                {
                    cryptocurrencyRecords = csv.GetRecords<CryptocurrencyDataDto>().ToList();
                }

                await _cryptocurrencyRepository.AddCryptocurrencyData(currencyCode, cryptocurrencyRecords);
                await _cryptocurrencyRepository.SaveChangesAsync();
            }
        }
        
        public async Task GetCryptoCurrencyData()
        {
            await Task.Delay(100);
        }
    }
}