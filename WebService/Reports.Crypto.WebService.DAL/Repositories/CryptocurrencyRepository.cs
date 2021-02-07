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
    public class CryptocurrencyRepository: GenericRepository<Cryptocurrency>, ICryptocurrencyRepository
    {
        private readonly CryptocurrenciesDbContext _context;

        public CryptocurrencyRepository(CryptocurrenciesDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task AddCryptocurrencyData(string currencyCode, List<CryptocurrencyDataDto> cryptocurrencyDataDtos)
        {
            var cryptocurrency = await this.All()
                .SingleAsync(c => c.Code == currencyCode);

            var cryptocurrenciesDataForAddition = cryptocurrencyDataDtos
                .Where(cdd => cryptocurrency.CryptocurrencyData.All(cd => cd.Date != cdd.Date));

            foreach (var singleCryptoDataForAddition in cryptocurrenciesDataForAddition)
            {
                var cryptocurrencyData = new CryptocurrencyData
                {
                    Date = singleCryptoDataForAddition.Date,
                    PriceUSD = singleCryptoDataForAddition.PriceUSD,
                };
                
                cryptocurrency.CryptocurrencyData.Add(cryptocurrencyData);
            }
            
            Update(cryptocurrency);
        }

        public async Task<List<CryptocurrencyDisplayDataDto>> GetCryptoCurrencyData(DateTime fromDate, DateTime toDate)
        {
            return await All().Join(
                    _context.CryptocurrencyData,
                    c => c.Id,
                    cd => cd.CryptocurrencyId, 
                    (c, cd) => new
                    {
                        c.Code,
                        cd.Date,
                        cd.PriceUSD
                    })
                .Where(c =>
                    c.PriceUSD.HasValue 
                    && c.Date >= fromDate 
                    && c.Date <= toDate)
                .OrderBy(c => c.Code)
                .ThenByDescending(c => c.Date)
                .Select(c => new CryptocurrencyDisplayDataDto
                {
                    Cryptocurrency = c.Code,
                    Date = c.Date,
                    PriceUSD = (decimal)c.PriceUSD
                })
                .ToListAsync();
        }
    }
}