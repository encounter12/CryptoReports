using System.Threading.Tasks;
using Reports.Crypto.WebService.DAL.Context;
using Reports.Crypto.WebService.DAL.Entities;
using Reports.Crypto.WebService.DAL.Repositories.Contracts;

namespace Reports.Crypto.WebService.DAL.Repositories
{
    public class CryptocurrencyRepository: GenericRepository<Cryptocurrency>, ICryptocurrencyRepository
    {
        private readonly CryptocurrenciesDbContext _context;

        public CryptocurrencyRepository(
            CryptocurrenciesDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddCryptocurrencyData()
        {
            await Task.Delay(100);
        }
    }
}