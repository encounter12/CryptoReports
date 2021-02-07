using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Crypto.WebService.DAL.Entities;
using Reports.Crypto.WebService.DTO;

namespace Reports.Crypto.WebService.DAL.Repositories.Contracts
{
    public interface ICryptocurrencyRepository: IGenericRepository<Cryptocurrency>
    {
        Task AddCryptocurrencyData(string currencyCode, List<CryptocurrencyDataDto> cryptocurrencyDataDtos);

        Task<List<CryptocurrencyDisplayDataDto>> GetCryptoCurrencyData(DateTime fromDate, DateTime toDate);
    }
}