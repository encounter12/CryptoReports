using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Crypto.WebService.DAL.Entities;
using Reports.Crypto.WebService.DTO;

namespace Reports.Crypto.WebService.DAL.Repositories.Contracts
{
    public interface ICryptocurrencyDataRepository: IGenericRepository<CryptocurrencyData>
    {
        Task<Cryptocurrency> GetCryptocurrencyByCode(string currencyCode);

        Task<IEnumerable<string>> AllCryptocurrenciesCodes();
        
        Task AddCryptocurrencyData(string currencyCode, IEnumerable<CryptocurrencyDataDto> cryptocurrencyDataDtos);

        Task<IEnumerable<CryptocurrencyDisplayDataDto>> GetCryptocurrencyData(DateTime fromDate, DateTime toDate);
    }
}