using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Crypto.WebService.DTO;

namespace Reports.Crypto.WebService.Services.Contracts
{
    public interface ICryptocurrencyDataService
    {
        Task AddCryptocurrencyData();

        Task<IEnumerable<CryptocurrencyDisplayDataDto>> GetCryptocurrencyData(DateTime fromDate, DateTime toDate);
    }
}