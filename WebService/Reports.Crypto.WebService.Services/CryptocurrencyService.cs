using System.Threading.Tasks;
using Reports.Crypto.WebService.DAL.Repositories.Contracts;
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
            await Task.Delay(100);
        }
        
        public async Task GetCryptoCurrencyData()
        {
            await Task.Delay(100);
        }
    }
}