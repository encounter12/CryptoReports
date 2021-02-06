using System.Collections.Generic;

namespace Reports.Crypto.WebService.DAL.Entities
{
    public class Cryptocurrency
    {
        public long Id { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public virtual ICollection<CryptocurrencyData> CryptocurrencyData { get; set; }
    }
}