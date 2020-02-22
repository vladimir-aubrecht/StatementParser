using StatementParser.Models;
using TaxReporterCLI.Models.Attributes;

namespace TaxReporterCLI.Models.Views
{
	public class SaleTransactionView : TransactionView
	{
        [Description("Profit in {ExchangedToCurrency} per Day")]
        public decimal? ExchangedPerDayProfit { get; }

        [Description("Profit in {ExchangedToCurrency} per Year")]
        public decimal? ExchangedPerYearProfit { get; }

        [Description("Swap in {ExchangedToCurrency} per Day")]
        public decimal? ExchangedPerDaySwap { get; }

        [Description("Swap in {ExchangedToCurrency} per Year")]
        public decimal? ExchangedPerYearSwap { get; }

        [Description("Taxes in {ExchangedToCurrency} per Day")]
        public decimal? ExchangedPerDayTaxes { get; }

        [Description("Taxes in {ExchangedToCurrency} per Year")]
        public decimal? ExchangedPerYearTaxes { get; }

        [Description("Commission in {ExchangedToCurrency} per Day")]
        public decimal? ExchangedPerDayCommission { get; }

        [Description("Commission in {ExchangedToCurrency} per Year")]
        public decimal? ExchangedPerYearCommission { get; }

        public SaleTransactionView(SaleTransaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear, Currency exchangedToCurrency) : base(transaction, exchangeRatePerDay, exchangeRatePerYear, exchangedToCurrency)
		{
            ExchangedPerDayProfit = transaction.Profit * exchangeRatePerDay;
            ExchangedPerYearProfit = transaction.Profit * exchangeRatePerYear;

            ExchangedPerDaySwap = transaction.Swap * exchangeRatePerDay;
            ExchangedPerYearSwap = transaction.Swap * exchangeRatePerYear;

            ExchangedPerDayTaxes = transaction.Taxes * exchangeRatePerDay;
            ExchangedPerYearTaxes = transaction.Taxes * exchangeRatePerYear;

            ExchangedPerDayCommission = transaction.Commission * exchangeRatePerDay;
            ExchangedPerYearCommission = transaction.Commission * exchangeRatePerYear;
        }

        public override string ToString()
        {
            return base.ToString() 
            + $" {nameof(ExchangedPerDayProfit)}: {ExchangedPerDayProfit} {nameof(ExchangedPerYearProfit)}: {ExchangedPerYearProfit} {nameof(ExchangedPerDaySwap)}: {ExchangedPerDaySwap} {nameof(ExchangedPerYearSwap)}: {ExchangedPerYearSwap}"
            + $" {nameof(ExchangedPerDayTaxes)}: {ExchangedPerDayTaxes} {nameof(ExchangedPerYearTaxes)}: {ExchangedPerYearTaxes} {nameof(ExchangedPerDayCommission)}: {ExchangedPerDayCommission} {nameof(ExchangedPerYearCommission)}: {ExchangedPerYearCommission}";
        }
	}
}