using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class ESPPTransactionView : TransactionView
	{
        public decimal? ExchangedPerDayTotalProfit { get; }
        public decimal? ExchangedPerYearTotalProfit { get; }

		public ESPPTransactionView(ESPPTransaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear) : base(transaction, exchangeRatePerDay, exchangeRatePerYear)
		{
            ExchangedPerDayTotalProfit = transaction.TotalProfit * exchangeRatePerDay;
            ExchangedPerYearTotalProfit = transaction.TotalProfit * exchangeRatePerYear;
		}

        public override string ToString()
        {
            return base.ToString() + $" {nameof(ExchangedPerDayTotalProfit)}: {ExchangedPerDayTotalProfit} {nameof(ExchangedPerYearTotalProfit)}: {ExchangedPerYearTotalProfit}";
        }
	}
}