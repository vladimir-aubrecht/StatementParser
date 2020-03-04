using StatementParser.Attributes;
using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class ESPPTransactionView : TransactionView
	{
        [Description("Total Profit in {ExchangedToCurrency} per Day")]
        public decimal? ExchangedPerDayTotalProfit { get; }

        [Description("Total Profit in {ExchangedToCurrency} per Year")]
        public decimal? ExchangedPerYearTotalProfit { get; }

        public ESPPTransactionView(ESPPTransaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear, Currency exchangedToCurrency) : base(transaction, exchangeRatePerDay, exchangeRatePerYear, exchangedToCurrency)
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