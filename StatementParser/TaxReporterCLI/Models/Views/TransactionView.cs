using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class TransactionView
	{
		public Transaction Transaction { get; }

		public decimal? ExchangeRatePerDay { get; }
		public decimal? ExchangeRatePerYear { get; }

		public TransactionView(Transaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear)
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