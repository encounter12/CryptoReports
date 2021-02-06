using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Reports.Crypto.WebService.DAL.Context;

namespace Reports.Crypto.WebService.DAL.Seed
{
    public class DbInitializer
    {
        public static CryptocurrenciesDbContext BuildDbContext(
            IConfigurationRoot config, string connectionStringName)
        {
            string cryptocurrenciesConnectionString = config.GetConnectionString(connectionStringName);
            
            var optionsBuilder = new DbContextOptionsBuilder<CryptocurrenciesDbContext>();

            optionsBuilder
                .UseSqlite(cryptocurrenciesConnectionString);
            
            var cryptocurrenciesDbContext = 
                new CryptocurrenciesDbContext(optionsBuilder.Options);

            return cryptocurrenciesDbContext;
        }

        public static async Task Seed(IConfigurationRoot config, string connectionStringName)
        {
            CryptocurrenciesDbContext dbContext = BuildDbContext(config, connectionStringName);
            await dbContext.Database.EnsureCreatedAsync();
        }
    }
}