using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reports.Crypto.WebService.DAL.Repositories.Contracts;
using Reports.Crypto.WebService.DTO;
using Reports.Crypto.WebService.Infrastructure.ExtensionMethods;
using Reports.Crypto.WebService.Services.Contracts;

namespace Reports.Crypto.WebService.Services
{
    public class CryptocurrencyDataService: ICryptocurrencyDataService
    {
        private readonly IServiceProvider _serviceProvider;
        
        private readonly HttpClient _httpClient;
        
        public CryptocurrencyDataService(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            _serviceProvider = serviceProvider;
            _httpClient = httpClient;
        }
        
        public async Task AddCryptocurrencyData()
        {
            var cryptocurrencyDataRepository = _serviceProvider.GetRequiredService<ICryptocurrencyDataRepository>();

            IEnumerable<string> cryptocurrenciesCodes = await cryptocurrencyDataRepository.AllCryptocurrenciesCodes();
            
            await cryptocurrenciesCodes.ForEachAsync(partitionCount: 10, AddDataForSingleCryptocurrency);
        }
        
        public async Task<IEnumerable<CryptocurrencyDisplayDataDto>> GetCryptocurrencyData(
            DateTime fromDate, DateTime toDate)
        {
            var cryptocurrencyDataRepository = _serviceProvider.GetRequiredService<ICryptocurrencyDataRepository>();
            
            IEnumerable<CryptocurrencyDisplayDataDto> cryptoCurrencyData = 
                await cryptocurrencyDataRepository.GetCryptocurrencyData(fromDate, toDate);

            return cryptoCurrencyData;
        }
        
        private async Task AddDataForSingleCryptocurrency(string currencyCode)
        {
            var url = $"https://coinmetrics.io/newdata/{currencyCode}.csv";
            
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Stream cryptocurrencyStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(cryptocurrencyStream);
                
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = (header, index) => header.ToLower()
                };
                
                IEnumerable<CryptocurrencyDataDto> cryptocurrencyRecords;
                
                using (var csvReader = new CsvReader(reader, config))
                {
                    cryptocurrencyRecords = csvReader.GetRecords<CryptocurrencyDataDto>().ToList();
                }
                
                var cryptocurrencyDataRepository = _serviceProvider.GetRequiredService<ICryptocurrencyDataRepository>();

                await cryptocurrencyDataRepository.AddCryptocurrencyData(currencyCode, cryptocurrencyRecords);
                await cryptocurrencyDataRepository.SaveChangesAsync();
            }
        }
    }
}