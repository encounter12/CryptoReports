using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Crypto.WebService.DAL.Context.EntityTypeConfiguration;
using Reports.Crypto.WebService.DAL.Entities;

namespace Reports.Crypto.WebService.DAL.Context
{
    public class CryptocurrenciesDbContext: DbContext
    {
        public CryptocurrenciesDbContext(DbContextOptions<CryptocurrenciesDbContext> options) : 
            base(options)
        {
        }
        
        public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
        
        public DbSet<CryptocurrencyData> CryptocurrencyData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new CryptocurrencyEntityTypeConfiguration().Configure(modelBuilder.Entity<Cryptocurrency>());
            new CryptocurrencyDataEntityTypeConfiguration().Configure(modelBuilder.Entity<CryptocurrencyData>());
        }
        
        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync(true, cancellationToken);
        }
    }
}