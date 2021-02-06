using System.Threading.Tasks;

namespace Reports.Crypto.WebService.Services.Contracts
{
    public interface ICryptocurrencyService
    {
        Task AddCryptoCurrencyData();

        Task GetCryptoCurrencyData();
    }
}