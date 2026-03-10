namespace BabCrm.Service.Models
{
    public class Currency
    {
        public string CurrencyId { get; set; }
        
        public string CurrencyName { get; set; }
        
        public string CurrencyCode { get; set; }
        
        public string CurrencySymbol { get; set; }
        
        public double ExchangeRate { get; set; }
        
        public double CurrencyPrecision { get; set; }
    }
}
