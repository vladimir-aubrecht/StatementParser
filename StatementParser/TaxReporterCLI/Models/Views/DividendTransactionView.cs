using StatementParser.Models;
using TaxReporterCLI.Models.Attributes;

namespace TaxReporterCLI.Models.Views
{
	public class DividendTransactionView : TransactionView
	{
        [Description("Income in {ExchangedToCurrency} per Day")]
        public decimal? ExchangedPerDayIncome { get; }

        [Description("Income in {ExchangedToCurrency} per Year")]
        public decimal? ExchangedPerYearIncome { get; }

        [Description("Tax in {ExchangedToCurrency} per Day")]
        public decimal? ExchangedPerDayTax { get; }

        [Description("Tax in {ExchangedToCurrency} per Year")]
        public decimal? ExchangedPerYearTax { get; }

		public DividendTransactionView(DividendTransaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear, Currency exchangedToCurrency) : base(transaction, exchangeRatePerDay, exchangeRatePerYear, exchangedToCurrency)
		{
            ExchangedPerDayIncome = transaction.Income * exchangeRatePerDay;
            ExchangedPerYearIncome = transaction.Income * exchangeRatePerYear;

            ExchangedPerDayTax = transaction.Tax * exchangeRatePerDay;
            ExchangedPerYearTax = transaction.Tax * exchangeRatePerYear;
        }

        public override string ToString()
        {
            return base.ToString() + $" {nameof(ExchangedPerDayIncome)}: {ExchangedPerDayIncome} {nameof(ExchangedPerYearIncome)}: {ExchangedPerYearIncome} {nameof(ExchangedPerDayTax)}: {ExchangedPerDayTax} {nameof(ExchangedPerYearTax)}: {ExchangedPerYearTax}";
        }
	}
}