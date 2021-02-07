using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Reports.Crypto.WebService.DAL.Context;
using Reports.Crypto.WebService.DAL.Entities;

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

            if (dbContext.Cryptocurrencies.Any())
            {
                return;
            }
            
            var cryptocurrencies = new List<Cryptocurrency>()
            {
                new Cryptocurrency { Code = "btc", Name = "Bitcoin" },
                new Cryptocurrency { Code = "bch", Name = "Bitcoin Cash" },
                new Cryptocurrency { Code = "bnb", Name = "Binance Coin" },
                new Cryptocurrency { Code = "ltc", Name = "Litecoin" },
                new Cryptocurrency { Code = "bsv", Name = "Bitcoin SV" },
                new Cryptocurrency { Code = "eth", Name = "Ethereum" },
                new Cryptocurrency { Code = "xrp", Name = "Ripple" }
            };
            
            await dbContext.Cryptocurrencies.AddRangeAsync(cryptocurrencies);
            await dbContext.SaveChangesAsync();
        }
    }
}