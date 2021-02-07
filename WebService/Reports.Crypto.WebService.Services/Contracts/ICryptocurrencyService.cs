using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Crypto.WebService.DTO;

namespace Reports.Crypto.WebService.Services.Contracts
{
    public interface ICryptocurrencyService
    {
        Task AddCryptocurrencyData();

        Task<List<CryptocurrencyDisplayDataDto>> GetCryptoCurrencyData(DateTime fromDate, DateTime toDate);
    }
}