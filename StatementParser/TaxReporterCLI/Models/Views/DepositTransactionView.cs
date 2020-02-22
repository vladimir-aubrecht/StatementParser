using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class DepositTransactionView : TransactionView
	{
        public decimal? ExchangedPerDayTotalPrice { get; }
        public decimal? ExchangedPerYearTotalPrice { get; }

		public DepositTransactionView(DepositTransaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear) : base(transaction, exchangeRatePerDay, exchangeRatePerYear)
		{
            ExchangedPerDayTotalPrice = transaction.TotalPrice * exchangeRatePerDay;
            ExchangedPerYearTotalPrice = transaction.TotalPrice * exchangeRatePerYear;
		}

        public override string ToString()
        {
            return base.ToString() + $" {nameof(ExchangedPerDayTotalPrice)}: {ExchangedPerDayTotalPrice} {nameof(ExchangedPerYearTotalPrice)}: {ExchangedPerYearTotalPrice}";
        }
	}
}