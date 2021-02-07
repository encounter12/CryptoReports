using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reports.Crypto.WebService.DAL.Repositories.Contracts;
using Reports.Crypto.WebService.DTO;
using Reports.Crypto.WebService.Infrastructure.ExtensionMethods;
using Reports.Crypto.WebService.Services.Contracts;

namespace Reports.Crypto.WebService.Services
{
    public class CryptocurrencyService: ICryptocurrencyService
    {
        private readonly IServiceProvider _serviceProvider;
        
        public CryptocurrencyService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task AddCryptoCurrencyData()
        {
            var cryptocurrencyRepository = _serviceProvider.GetRequiredService<ICryptocurrencyRepository>();
            
            var currencyCodes = await cryptocurrencyRepository
                .All()
                .Select(c => c.Code)
                .ToListAsync();
            
            await currencyCodes.ForEachAsync(7, AddDataForSingleCryptocurrency);
        }
        
        public async Task GetCryptoCurrencyData()
        {
            await Task.Delay(100);
        }
        
        private async Task AddDataForSingleCryptocurrency(string currencyCode)
        {
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
                
                List<CryptocurrencyDataDto> cryptocurrencyRecords;
                
                using (var csv = new CsvReader(reader, config))
                {
                    cryptocurrencyRecords = csv.GetRecords<CryptocurrencyDataDto>().ToList();
                }
                
                var cryptocurrencyRepository = _serviceProvider.GetRequiredService<ICryptocurrencyRepository>();

                await cryptocurrencyRepository.AddCryptocurrencyData(currencyCode, cryptocurrencyRecords);
                await cryptocurrencyRepository.SaveChangesAsync();
            }
        }
    }
}