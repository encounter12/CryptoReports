using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Crypto.WebService.DAL.Entities;

namespace Reports.Crypto.WebService.DAL.Context.EntityTypeConfiguration
{
    public class CryptocurrencyDataEntityTypeConfiguration : IEntityTypeConfiguration<CryptocurrencyData>
    {
        public void Configure(EntityTypeBuilder<CryptocurrencyData> builder)
        {
            builder
                .Property(cd => cd.Id)
                .IsRequired();
            
            builder
                .Property(cd => cd.Date)
                .IsRequired();
            
            builder
                .HasOne(cd => cd.Cryptocurrency)
                .WithMany(c => c.CryptocurrencyData)
                .HasForeignKey(a => a.CryptocurrencyId)
                .IsRequired();
            
            builder.Property(cd =>  cd.PriceUSD)
                .HasConversion<double>();
        }
    }
}