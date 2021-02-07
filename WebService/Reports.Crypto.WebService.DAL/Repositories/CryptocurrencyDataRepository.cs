using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Crypto.WebService.DAL.Context;
using Reports.Crypto.WebService.DAL.Entities;
using Reports.Crypto.WebService.DAL.Repositories.Contracts;
using Reports.Crypto.WebService.DTO;

namespace Reports.Crypto.WebService.DAL.Repositories
{
    public class CryptocurrencyDataRepository: GenericRepository<CryptocurrencyData>, ICryptocurrencyDataRepository
    {
        private readonly CryptocurrenciesDbContext _context;

        public CryptocurrencyDataRepository(CryptocurrenciesDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cryptocurrency> GetCryptocurrencyByCode(string currencyCode)
        {
            return await _context.Cryptocurrencies.SingleAsync(c => c.Code == currencyCode);
        }
        
        public async Task<IEnumerable<string>> AllCryptocurrenciesCodes()
        {
            return await _context.Cryptocurrencies.Select(c => c.Code).ToListAsync();
        }
        
        public async Task AddCryptocurrencyData(
            string currencyCode,
            IEnumerable<CryptocurrencyDataDto> cryptocurrencyDataDtos)
        {
            var cryptocurrency = await GetCryptocurrencyByCode(currencyCode);
            
            var cryptocurrenciesDataForAddition = cryptocurrencyDataDtos
                .Where(cdd => cryptocurrency.CryptocurrencyData.All(cd => cd.Date != cdd.Date));

            foreach (var singleCryptoDataForAddition in cryptocurrenciesDataForAddition)
            {
                var cryptocurrencyData = new CryptocurrencyData
                {
                    Date = singleCryptoDataForAddition.Date,
                    PriceUSD = singleCryptoDataForAddition.PriceUSD,
                    Cryptocurrency = cryptocurrency
                };
                
                Add(cryptocurrencyData);
            }
        }

        public async Task<IEnumerable<CryptocurrencyDisplayDataDto>> GetCryptocurrencyData(
            DateTime fromDate, DateTime toDate)
        {
            return await All()
                .Where(cd =>
                    cd.PriceUSD.HasValue
                    && cd.Date >= fromDate
                    && cd.Date <= toDate)
                .OrderBy(cd => cd.Cryptocurrency.Code)
                .ThenByDescending(cd => cd.Date)
                .Select(cd => new CryptocurrencyDisplayDataDto
                {
                    Cryptocurrency = cd.Cryptocurrency.Code,
                    Date = cd.Date,
                    PriceUSD = (decimal)cd.PriceUSD
                })
                .ToListAsync();
        }
    }
}