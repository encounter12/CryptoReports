using System.Threading.Tasks;
using Reports.Crypto.WebService.DAL.Entities;

namespace Reports.Crypto.WebService.DAL.Repositories.Contracts
{
    public interface ICryptocurrencyRepository: IGenericRepository<Cryptocurrency>
    {
        Task AddCryptocurrencyData();
    }
}