// ReSharper disable InconsistentNaming

using System;

namespace Reports.Crypto.WebService.DTO
{
    public class CryptocurrencyDataDto
    {
        public DateTime Date { get; set; }
        
        public decimal? PriceUSD { get; set; }
    }
}