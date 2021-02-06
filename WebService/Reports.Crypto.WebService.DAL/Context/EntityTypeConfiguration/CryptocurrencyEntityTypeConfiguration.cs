using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Crypto.WebService.DAL.Entities;

namespace Reports.Crypto.WebService.DAL.Context.EntityTypeConfiguration
{
    public class CryptocurrencyEntityTypeConfiguration : IEntityTypeConfiguration<Cryptocurrency>
    {
        public void Configure(EntityTypeBuilder<Cryptocurrency> builder)
        {
            builder
                .Property(c => c.Id)
                .IsRequired();
            
            builder
                .Property(c => c.Code)
                .IsRequired();
            
            builder
                .Property(c => c.Name)
                .IsRequired();
        }
    }
}