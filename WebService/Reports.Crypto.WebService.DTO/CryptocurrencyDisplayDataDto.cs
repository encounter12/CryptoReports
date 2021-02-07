// ReSharper disable InconsistentNaming
using System;

namespace Reports.Crypto.WebService.DTO
{
    public class CryptocurrencyDisplayDataDto
    {
        public string Cryptocurrency { get; set; }
        
        public DateTime Date { get; set; }
        
        public decimal PriceUSD { get; set; }
    }
}