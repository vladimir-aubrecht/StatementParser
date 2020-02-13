using StatementParser.Models;

namespace TaxReporterCLI
{
    public class EnrichedTransaction
    {
        public Transaction Transaction { get; }

        public decimal ExchangeRatePerDay;
        public decimal ExchangeRatePerYear;

        public EnrichedTransaction(Transaction transaction, decimal exchangeRatePerDay, decimal exchangeRatePerYear)
        {
            this.Transaction = transaction;
            this.ExchangeRatePerDay = exchangeRatePerDay;
            this.ExchangeRatePerYear = exchangeRatePerYear;
        }

        public override string ToString() 
        {
            return $"{Transaction} {nameof(ExchangeRatePerDay)}: {ExchangeRatePerDay} {nameof(ExchangeRatePerYear)}: {ExchangeRatePerYear}";
        }
    }
}