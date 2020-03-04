using StatementParser.Attributes;
using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class DepositTransactionView : TransactionView
	{
        [Description("Total Price in {ExchangedToCurrency} per Day")]
        public decimal? ExchangedPerDayTotalPrice { get; }

        [Description("Total Price in {ExchangedToCurrency} per Year")]
        public decimal? ExchangedPerYearTotalPrice { get; }

		public DepositTransactionView(DepositTransaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear, Currency exchangedToCurrency) : base(transaction, exchangeRatePerDay, exchangeRatePerYear, exchangedToCurrency)
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