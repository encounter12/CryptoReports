// ReSharper disable InconsistentNaming
using System;

namespace Reports.Crypto.WebService.DAL.Entities
{
    public class CryptocurrencyData
    {
        public long Id { get; set; }
        
        public long CryptocurrencyId { get; set; }
        
        public virtual Cryptocurrency Cryptocurrency { get; set; }
        
        public DateTime Date { get; set; }
        
        public decimal? PriceUSD { get; set; }
    }
}